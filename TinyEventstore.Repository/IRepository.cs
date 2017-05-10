using System.Data;

namespace TinyEventstore.Producer
{
    public interface IRepository
    {
        T GetById<T>(string id);
        void Save(IAggregate aggregate);
    }
}
