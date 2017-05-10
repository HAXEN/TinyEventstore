using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace TinyEventstore.Producer.Tests
{
    public class DatabaseManagementTests
    {
        private string _connectionString = "Data Source=RRY-PC;Initial Catalog=eventstore;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True";


        [Fact]
        public void Should_be_able_to_Purge_Database()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var bdManager = new DatabaseManagement(connection);

                bdManager.Purge();

                connection.Close();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(Offset) FROM Events";
                command.CommandType = CommandType.Text;

                Assert.Equal(0, command.ExecuteScalar());

                connection.Close();
            }
        }
    }
}
