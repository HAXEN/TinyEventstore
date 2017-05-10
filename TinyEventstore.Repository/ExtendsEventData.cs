using System.Collections.Generic;
using Newtonsoft.Json;

namespace TinyEventstore.Producer
{
    public static class ExtendsEventData
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
        };

        public static EventData ToEventData(this object @event, string aggregateId, string aggregateType, int version)
        {
            var body = JsonConvert.SerializeObject(@event, SerializerSettings);
            var eventHeader = new Dictionary<string, object>()
            {
                { "EventClrType", @event.GetType().AssemblyQualifiedName },
                { "EventName", @event.GetType().Name },
            };
            var header = JsonConvert.SerializeObject(eventHeader, SerializerSettings);

            return new EventData
            {
                AggregateId = aggregateId,
                AggregateType = aggregateType,
                Version = version,
                Header = header,
                Body = body,
            };
        }
    }
}