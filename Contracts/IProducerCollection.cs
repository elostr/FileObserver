
namespace FileObserver.Contracts
{
    public interface IProducerCollection
    {
         bool TryAdd(FileTask fileTask);
    }
}
