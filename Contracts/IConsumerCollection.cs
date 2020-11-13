using System.Threading;

namespace FileObserver.Contracts
{
    public interface IConsumerCollection
    {
       AutoResetEvent TaskAdded { get; }

       bool TryTake(out string filePath);

    }
}
