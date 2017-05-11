using System;
using Xunit;

namespace TinyEventstore.Consumer.Tests
{
    public class Consumer_Tests
    {
        private string _connectionString = "Data Source=RRY-PC;Initial Catalog=eventstore;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True";

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
                    observeCommits.Start().Wait(TimeSpan.FromSeconds(1));
                }
            }
        }
    }
}
