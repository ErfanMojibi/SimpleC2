using TeamServer.Models.Agents;
using TeamServer.Models.Listeners;

namespace TeamServer.Services
{
    public interface IAgentService
    {
        void AddAgent(Agent agent);
        void RemoveAgent(Agent agent);
        IEnumerable<Agent> GetAllAgents();

        Agent GetAgent(string id);
    }
}
