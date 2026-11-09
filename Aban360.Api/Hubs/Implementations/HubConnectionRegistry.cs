using System.Collections.Concurrent;

namespace Aban360.Api.Hubs.Implementations
{
    public sealed class HubConnectionRegistry
    {
        private readonly ConcurrentDictionary<string, byte> _connections = new(StringComparer.Ordinal);

        public void Add(string connectionId) => _connections.TryAdd(connectionId, 0);
        public void Remove(string connectionId) => _connections.TryRemove(connectionId, out _);
        public string[] GetConnectionIds() => _connections.Keys.ToArray();
    }
}
