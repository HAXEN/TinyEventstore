using System;

namespace TinyEventstore.Producer
{
    public class DatabaseManagementException : Exception
    {
        public DatabaseManagementException(string message):base(message) {}
    }
}