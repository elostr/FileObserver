using System;
using System.Collections.Concurrent;
using System.Threading;
using FileObserver.Contracts;

namespace FileObserver
{
    public class FileTaskCollection : IProducerCollection, IConsumerCollection
    {
        private readonly IProducerConsumerCollection<FileTask> _collection = new ConcurrentQueue<FileTask>();

        private readonly object _object = new object();

        private readonly AutoResetEvent _taskAdded = new AutoResetEvent(initialState: false);

        public bool TryAdd(FileTask fileTask)
        {
            lock (_object)
            {
                var rval = _collection.TryAdd(fileTask);
                if (rval)
                {
                    _taskAdded.Set();
                }

                return rval;
            }
        }

        public AutoResetEvent TaskAdded => _taskAdded;

        public bool TryTake(out FileTask fileTask)
        {
            return _collection.TryTake(out fileTask);
        }        
    }
}
