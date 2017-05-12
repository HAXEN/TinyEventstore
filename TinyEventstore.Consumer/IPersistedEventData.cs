using System;

namespace TinyEventstore.Consumer
{
    public interface IPersistedEventData
    {
        long Offset { get; }
        string AggregateId { get; }
        int Version { get; }
        string AggregateType { get; }
        DateTime Timestamp { get; }
        string Header { get; }
        string Body { get; }
    }

    internal class PersistedEventData : IPersistedEventData
    {
        public long Offset { get; set; }
        public string AggregateId { get; set; }
        public int Version { get; set; }
        public string AggregateType { get; set; }
        public DateTime Timestamp { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
    }
}