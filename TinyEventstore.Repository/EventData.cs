namespace TinyEventstore.Producer
{
    public class EventData
    {
        public string AggregateId { get; set; }
        public string AggregateType { get; set; }
        public int Version { get; set; }
        public string Body { get; set; }
        public string Header { get; set; }
    }
}