using TeamServer.Models;

namespace TeamServer.Services
{
    public class ListenerService : IListenerService 
    {
        private readonly List<Listener> _listeners = new List<Listener>();

        public void AddListener(Listener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(Listener listener)
        {
            _listeners.Remove(listener);
        }

        public IEnumerable<Listener> GetAllListeners()
        {
            return _listeners;
        }

        public Listener GetListener(string name)
        {
            return GetAllListeners().FirstOrDefault(l => l.Name.Equals(name));
        }
    }
}
