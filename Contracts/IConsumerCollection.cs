using System.Threading;

namespace FileObserver.Contracts
{
    /// <summary>
    /// интерфейс шины данных для consumer
    /// </summary>
    public interface IConsumerCollection
    {
       AutoResetEvent TaskAdded { get; }

       bool TryTake(out string filePath);

    }
}
