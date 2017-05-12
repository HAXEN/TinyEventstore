using System;
using System.Data;
using System.Data.SqlClient;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

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
            private long _offset;
            private readonly Subject<IPersistedEventData> _subject = new Subject<IPersistedEventData>();
            private readonly CancellationTokenSource _stopRequesting = new CancellationTokenSource();
            private TaskCompletionSource<Unit> _runningTaskCompletionSource;
            private int _isPolling = 0;
            private SqlConnection _connection;

            public PollingObserver(string connectionString, int interval, long offset)
            {
                _connectionString = connectionString;
                _interval = interval;
                _offset = offset;
            }

            private SqlConnection Connection()
            {
                if(_connection == null)
                    _connection = new SqlConnection(_connectionString);

                switch (_connection.State)
                {
                     case ConnectionState.Closed:
                        _connection.Open();
                        break;
                }

                return _connection;
            }

            private void PollEvents()
            {
                if (Interlocked.CompareExchange(ref _isPolling, 1, 0) == 0)
                {
                    try
                    {
                        var events = Connection().Query<PersistedEventData>("SELECT * FROM Events WHERE Offset>@Offset ORDER BY Offset", new{ Offset = _offset});

                        foreach (var @event in events)
                        {
                            if (_stopRequesting.IsCancellationRequested)
                            {
                                _subject.OnCompleted();
                                return;
                            }
                            _subject.OnNext(@event);
                            _offset = @event.Offset;
                        }
                    }
                    catch (Exception exception)
                    {
                        _subject.OnError(exception);
                    }
                    Interlocked.Exchange(ref _isPolling, 0);
                }
            }

            public Task Start()
            {
                if (_runningTaskCompletionSource != null)
                    return _runningTaskCompletionSource.Task;

                _runningTaskCompletionSource = new TaskCompletionSource<Unit>();
                PollLoop();
                return _runningTaskCompletionSource.Task;
            }

            private void PollLoop()
            {
                if (_stopRequesting.IsCancellationRequested)
                {
                    Dispose();
                    return;
                }

                Task.Delay(_interval, _stopRequesting.Token)
                    .WhenCompleted(_ =>
                    {
                        PollEvents();
                        PollLoop();
                    }, _ => Dispose());
            }

            public IDisposable Subscribe(IObserver<IPersistedEventData> observer)
            {
                return _subject.Subscribe(observer);
            }

            public void Dispose()
            {
                _stopRequesting.Cancel();
                _subject.Dispose();
                _runningTaskCompletionSource?.TrySetResult(new Unit());
            }
        }
    }
}