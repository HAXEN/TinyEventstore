using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace TinyEventstore.Consumer
{
    public class PollingClient
    {
        private readonly string _connectionString;
        private readonly int _interval;

        public PollingClient(string connectionString, int interval)
        {
            _connectionString = connectionString;
            _interval = interval;
        }

        public IObserveEventstream ObserveFrom(int offset)
        {
            return new PollingObserver(_connectionString, _interval, offset);
        }

        private class PollingObserver : IObserveEventstream
        {
            private readonly string _connectionString;
            private readonly int _interval;
            private readonly int _offset;
            //private readonly ISubject<IPersistedEventData> _subject = Subject.;

            public PollingObserver(string connectionString, int interval, int offset)
            {
                _connectionString = connectionString;
                _interval = interval;
                _offset = offset;
            }


            public Task Start()
            {
                throw new NotImplementedException();
            }

            public IDisposable Subscribe(IObserver<IPersistedEventData> observer)
            {
                return null;
            }

            public void Dispose()
            {
            }
        }
    }
}