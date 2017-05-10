using System;

namespace TinyEventstore.Producer
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException() :base("Some other thread has saved data. Your Aggregate was not up to date.") {}
    }
}