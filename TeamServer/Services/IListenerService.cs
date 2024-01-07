using TeamServer.Models;

namespace TeamServer.Services
{
    public interface IListenerService
    {
        void AddListener(Listener listener);
        void RemoveListener(Listener listener);
        IEnumerable<Listener> GetAllListeners();

        Listener GetListener(string name);

    }
}
