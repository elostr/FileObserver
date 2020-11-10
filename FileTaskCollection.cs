using System;
using System.Collections.Concurrent;
using System.Threading;
using FileObserver.Contracts;

namespace FileObserver
{
    public class FileTaskCollection : IProducerCollection, IConsumerCollection
    {
        private readonly ConcurrentQueue<FileTask> _collection = new ConcurrentQueue<FileTask>();

        private readonly object _object = new object();

        public void Add(FileTask fileTask)
        {
            _collection.Enqueue(fileTask);
            TaskAdded.Set();
        }

        public AutoResetEvent TaskAdded { get; } = new AutoResetEvent(initialState: false);

        public bool TryTake(out FileTask fileTask)
        {
            return _collection.TryDequeue(out fileTask);
        }        
    }
}
