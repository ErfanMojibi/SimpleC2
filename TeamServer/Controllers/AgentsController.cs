using ApiModels.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamServer.Models.Agents;
using TeamServer.Models.Listeners;
using TeamServer.Services;

namespace TeamServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentService _agents;

        public AgentsController(IAgentService agents)
        {
            _agents = agents;
        }

        [HttpGet]
        public IActionResult GetAgents()
        {
            var agents = _agents.GetAllAgents();
            return Ok(agents);
        }

        [HttpGet("{agentId}")]
        public IActionResult GetAgent(string agentId)
        {
            var agent = _agents.GetAgent(agentId);
            if (agent is null)
                return NotFound();
            return Ok(agent);
        }

        [HttpGet("{agentId}/tasks/{taskId}/result")]
        public IActionResult GetAgentTaskResult(string agentId, string taskId)
        {
            var agent = _agents.GetAgent(agentId);
            if (agent is null)
                return NotFound("Agent not found");
            var taskResults = agent.GetTaskResult(taskId);
            if (taskResults is null)
                return NotFound("Task not found");

            return Ok(taskResults);
        }

        [HttpGet("{agentId}/tasks/")]
        public IActionResult GetAgentAllTask(string agentId)
        {
            var agent = _agents.GetAgent(agentId);
            if (agent is null)
                return NotFound("Agent not found");

            return Ok(agent.GetPendingTasks());
        }



        [HttpPost("{agentId}")]
        public IActionResult TaskAgent(string agentId, [FromBody] TaskAgentRequest taskAgentRequest)
        {
            var agent = _agents.GetAgent(agentId);
            if (agent is null)
                return NotFound();

            var task = new AgentTask()
            {
                Id = Guid.NewGuid().ToString(),
                Command = taskAgentRequest.Command,
                Arguments = taskAgentRequest.Arguments,
                File = taskAgentRequest.File,
            };

            agent.QueueTask(task);
            var root = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
            var path = $"{root}/tasks/{task.Id}";

            return Created(path, task);

        }
    }
}
