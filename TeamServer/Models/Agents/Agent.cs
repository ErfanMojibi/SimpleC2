using System.Collections.Concurrent;

namespace TeamServer.Models.Agents
{
    public class Agent
    {
        private readonly ConcurrentQueue<AgentTask> _pendingTasks = new();
        private readonly List<AgentTaskResult> _taskResults = new();

        public AgentMetadata Metadata { get; }
        public DateTime LastSeen { get; private set; }


        public Agent(AgentMetadata metadata)
        {
            Metadata = metadata;
        }

        public void CheckIn()
        {
            LastSeen = DateTime.UtcNow;
        }

        public IEnumerable<AgentTask> GetPendingTasks()
        {
            List<AgentTask> tasks = new();
            while (_pendingTasks.TryDequeue(out var task))
            {
                tasks.Add(task);
            }
            return tasks;
        }

        public void QueueTask(AgentTask task)
        {
            _pendingTasks.Enqueue(task);
        }

        public AgentTaskResult? GetTaskResult(string id)
        {
            return GetAllAgentTaskResults().FirstOrDefault(r => r.Id.Equals(id));

        }

        public IEnumerable<AgentTaskResult> GetAllAgentTaskResults()
        {
            return _taskResults;
        }

        public void AddTaskResults(IEnumerable<AgentTaskResult> results)
        {
            _taskResults.AddRange(results);
        }


    }
}
