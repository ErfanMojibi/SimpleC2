
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TeamServer.Models.Agents;
using TeamServer.Services;

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

        public IActionResult HandleImplant()
        {

            var metadata = ExtractMetadata(HttpContext.Request.Headers);


            return Ok("Your Listener works");
        }


        private AgentMetadata? ExtractMetadata(IHeaderDictionary header)
        {
            if (!header.TryGetValue("Authorization", out var encodedMetadata))
                return null;

            // Authorization: Bearer base64
            encodedMetadata = encodedMetadata.ToString().Substring(0, 7);
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(encodedMetadata));
            return JsonConvert.DeserializeObject<AgentMetadata>(json);
        }

    }
}
