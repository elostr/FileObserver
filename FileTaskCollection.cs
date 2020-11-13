using System.Collections.Concurrent;
using System.Threading;
using FileObserver.Contracts;

namespace FileObserver
{
    public class FileTaskCollection : IProducerCollection, IConsumerCollection
    {
        private readonly ConcurrentQueue<string> _collection = new ConcurrentQueue<string>();

        public void Add(string filePath)
        {
            _collection.Enqueue(filePath);
            TaskAdded.Set();
        }

        public AutoResetEvent TaskAdded { get; } = new AutoResetEvent(initialState: false);

        public bool TryTake(out string filePath)
        {
            return _collection.TryDequeue(out filePath);
        }        
    }
}
