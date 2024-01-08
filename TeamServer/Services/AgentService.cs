using TeamServer.Models.Agents;

namespace TeamServer.Services
{
    public class AgentService : IAgentService
    {
        private readonly List<Agent> _agents =  new List<Agent>();
        public void AddAgent(Agent agent)
        {
            _agents.Add(agent);
        }

        public void RemoveAgent(Agent agent)
        {
            _agents.Remove(agent);
        }

        public IEnumerable<Agent> GetAllAgents()
        {
            return _agents;
        }

        public Agent GetAgent(string id)
        {
            return _agents.FirstOrDefault(e => e.Metadata.Id.Equals(id));
        }
    }
}
