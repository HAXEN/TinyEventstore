using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace TinyEventstore.Producer
{
    public class DatabaseManagement
    {
        private readonly SqlConnection _connection;

        public DatabaseManagement(SqlConnection connection)
        {
            _connection = connection;
        }

        public void Purge()
        {
            Drop();
            Create();
        }

        public void Create()
        {
            if(TableExists())
                return;

            var command = _connection.CreateCommand();
            command.CommandText = CreateEventsTableScript();
            command.CommandType = CommandType.Text;

            var affected = command.ExecuteNonQuery();

            if (affected != -1)
                throw new DatabaseManagementException($"Affected {affected} expected -1.");
        }

        public void Drop()
        {
            if(TableExists() == false)
                return;

            var command = _connection.CreateCommand();
            command.CommandText = "DROP TABLE Events";
            command.CommandType = CommandType.Text;

            var affected = command.ExecuteNonQuery();

            if(affected != -1)
                throw new DatabaseManagementException($"Affected {affected} expected -1.");
        }

        public bool TableExists()
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT case when exists(SELECT* FROM information_schema.tables WHERE table_name = 'Events') then 1 else 0 end";
            command.CommandType = CommandType.Text;

            return command.ExecuteScalar().Equals(1);
        }

        private string CreateEventsTableScript()
        {
            var assembly = typeof(IRepository).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("TinyEventstore.Producer.CreateEventsTable.txt");

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}