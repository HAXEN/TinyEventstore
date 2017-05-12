using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TinyEventstore.Producer;
using Xunit;

namespace TinyEventstore.Consumer.Tests
{
    public class Consumer_Tests
    {
        private string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=eventstore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public Consumer_Tests()
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
        public void Should_get_the_event_when_committed()
        {
            var offset = 0L;
            var client = new PollingClient(_connectionString, 50);
            using (var observeCommits = client.ObserveFrom(0))
            {
                using (observeCommits.Subscribe(commit =>
                {
                    offset = commit.Offset;
                }))
                {
                    var consumerTask = observeCommits.Start();

                    using(var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        var producer = new Repository(connection);
                        producer.Save(new TestAggregate("Test1"));
                        producer.Save(new TestAggregate("Test2"));

                        connection.Close();
                    }

                    consumerTask.Wait(TimeSpan.FromMilliseconds(500));
                }
            }
            Assert.NotEqual(0L, offset);
            Assert.Equal(2, offset);
        }
        public class TestAggregate : IAggregate
        {
            public TestAggregate(string name)
            {
                Id = name;
            }

            public IEnumerable<object> GetUncommittedEvents()
            {
                yield return new TestAggregateCreated
                {
                    Description = "Description",
                    Name = "Test Aggregate",
                };
            }

            public int Version => 1;
            public string Id { get; set; }
            public void ClearUncommitted()
            {
            }
        }

        public class TestAggregateCreated
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
