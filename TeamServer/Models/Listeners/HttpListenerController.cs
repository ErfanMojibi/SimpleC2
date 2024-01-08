
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TeamServer.Models.Agents;
using TeamServer.Services;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

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

                var result = JsonConvert.DeserializeObject<IEnumerable<AgentTaskResult>>(json);
                agent.AddTaskResults(result);
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
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(encodedMetadata));
            return JsonConvert.DeserializeObject<AgentMetadata>(json);
        }

    }
}
