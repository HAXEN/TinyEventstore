using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace TinyEventstore.Producer.Tests
{
    public class Repository_Save_Tests
    {
        private string _connectionString =
            "Data Source=RRY-PC;Initial Catalog=eventstore;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True";

        public Repository_Save_Tests()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var manager = new DatabaseManagement(connection);
                manager.Purge();
                connection.Close();
            }
        }

        [Fact]
        public void Should_be_able_to_Store_event()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var repository = new Repository(connection);

                repository.Save(new TestEntity());

                connection.Close();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT MAX(Offset) FROM Events";
                command.CommandType = CommandType.Text;

                Assert.Equal(1L, command.ExecuteScalar());

                connection.Close();
            }
        }

        public class TestEntity : IAggregate
        {
            public IEnumerable<object> GetUncommittedEvents()
            {
                yield return new TestEntityCreated
                {
                    Id = "testkalle",
                    Name = "Test Kalle",
                };
            }

            public int Version => 1;
            public string Id => "testkalle";

            public void ClearUncommitted()
            {
            }
        }

        public class TestEntityCreated
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}