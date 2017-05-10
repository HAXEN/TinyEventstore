using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace TinyEventstore.Producer
{
    public class Repository : IRepository
    {
        private readonly SqlConnection _connection;

        public Repository(SqlConnection connection)
        {
            _connection = connection;
        }

        public T GetById<T>(string id)
        {
            return default(T);
        }

        public void Save(IAggregate aggregate)
        {
            if(aggregate == null)
                return;

            var events = aggregate.GetUncommittedEvents().ToArray();
            if(events.Any() == false)
                return;

            var aggregateType = aggregate.GetType().Name;
            var originalVersion = aggregate.Version - events.Count() + 1;
            var eventsToSave = events.Select(x => x.ToEventData(aggregate.Id, aggregateType, originalVersion++)).ToArray();

            using (var tx = _connection.BeginTransaction())
            {
                var foundVersion = (int?) _connection.ExecuteScalar("SELECT MAX(Version) FROM Events WHERE AggregateId=@AggregateId", new { AggregateId = aggregate.Id }, tx);
                if(foundVersion.GetValueOrDefault() >= originalVersion)
                    throw new ConcurrencyException();

                const string sql = @"INSERT INTO EVENTS(AggregateId, Version, AggregateType, Header, Body)
                                     VALUES(@AggregateId, @Version, @AggregateType, @Header, @Body)";

                _connection.Execute(sql, eventsToSave, tx);
                tx.Commit();
            }
            aggregate.ClearUncommitted();
        }
    }
}