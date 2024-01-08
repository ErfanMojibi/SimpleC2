using TeamServer.Services;

namespace TeamServer.Models.Listeners
{
    public abstract class Listener
    {
        protected IAgentService AgentService;
        public abstract string Name { get; }

        public void Init(IAgentService agentService)
        {
            AgentService = agentService;

        }
        public abstract Task Start();

        public abstract void Stop();
    }
}

