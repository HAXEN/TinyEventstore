using System;
using System.Reactive;
using System.Threading.Tasks;

namespace TinyEventstore.Consumer
{
    public interface IObserveEventstream : IObservable<IPersistedEventData>, IDisposable
    {
        Task Start();
    }
}