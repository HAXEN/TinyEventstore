using System.Collections.Generic;

namespace TinyEventstore.Producer
{
    public interface IAggregate
    {
        IEnumerable<object> GetUncommittedEvents();
        int Version { get; }
        string Id { get; }
        void ClearUncommitted();
    }
}