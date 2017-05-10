namespace TinyEventstore.Consumer
{
    public interface IPersistedEventData
    {
        long Offset { get; }
    }
}