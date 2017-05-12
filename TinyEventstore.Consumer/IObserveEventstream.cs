using System;
using System.Threading.Tasks;

namespace TinyEventstore.Consumer
{
    public interface IObserveEventstream : IObservable<IPersistedEventData>, IDisposable
    {
        Task Start();
    }
}