using System.Data.SqlClient;
using Xunit;

namespace TinyEventstore.Producer.Tests
{
    public class Repository_Read_Tests
    {
        private string _connectionString = "Data Source=RRY-PC;Initial Catalog=eventstore;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True";

        public Repository_Read_Tests()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var manager = new DatabaseManagement(connection);
                manager.Purge();
                connection.Close();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var repository = new Repository(connection);


                connection.Close();
            }
        }


        [Fact]
        public void Should_be_able_to_Load_Aggregate()
        {
            
        }
    }
}
