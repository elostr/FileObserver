using System.Threading;
using System.Threading.Tasks;
using FileObserver.Contracts;

namespace FileObserver
{
    public class Consumer
    {
        private readonly IConsumerCollection _collection;

        private readonly IFileWorker _worker;

        public Consumer(IConsumerCollection collection, IFileWorker worker)
        {
            _collection = collection;
            _worker = worker;
        }

        public void Start(CancellationToken token)
        {
            Task task = new Task(() => Execute(token));
            task.Start();
        }

        private void Execute(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {                
                while (_collection.TryTake(out var task) && !token.IsCancellationRequested)
                {
                    _worker.Work(task);
                }

                WaitHandle.WaitAny(waitHandles: new[] { token.WaitHandle, _collection.TaskAdded });              
            }
        }        
    }
}
