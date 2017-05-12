using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
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
            private readonly IObservable<IPersistedEventData> _events;
            private readonly IScheduler _scheduler;

            public PollingObserver(string connectionString, int interval, int offset)
            {
                _connectionString = connectionString;
                _interval = interval;
                _offset = offset;

                _scheduler = new EventLoopScheduler();
                
                _events = SetupListener();


            }

            private IObservable<IPersistedEventData> SetupListener()
            {
                return Observable.Create<IPersistedEventData>(o =>
                    {
                        return _scheduler.Schedule(TimeSpan.FromMilliseconds(_interval), (interval, recurse) =>
                        {
                            try
                            {
                                var events = PollEvents();
                                foreach (var @event in events)
                                {
                                    o.OnNext(@event);
                                }
                                recurse(interval);
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception);
                                o.OnError(exception);
                            }
                        });
                    });
            }

            private IEnumerable<IPersistedEventData> PollEvents()
            {
                yield return null;
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