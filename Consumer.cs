using System.Threading;
using System.Threading.Tasks;
using FileObserver.Contracts;

namespace FileObserver
{
    public class Consumer
    {
        private readonly IConsumerCollection _collection;

        private readonly IFileWorker _worker;

        private readonly IResultsWriter _writer;

        private readonly CancellationTokenSource _ctSource;

        private Task _task;

        public Consumer(IConsumerCollection collection, IFileWorker worker, IResultsWriter writer)
        {
            _collection = collection;
            _worker = worker;
            _writer = writer;

            _ctSource = new CancellationTokenSource();
        }

        public void Start()
        {
            if (_task != null)
            {
                return;
            }

            _task = Task.Run(() => Execute(_ctSource.Token));
        }

        public void Stop()
        {
            if (_task == null)
            {
                return;
            }

            _ctSource.Cancel();
            _task.Wait();
            _task.Dispose();
            _task = null;
        }

        private void Execute(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {                
                while (_collection.TryTake(out var task) && !token.IsCancellationRequested)
                {
                    int charCount = _worker.Work(task);
                    _writer.Write(task, charCount);
                }

                WaitHandle.WaitAny(new[] { token.WaitHandle, _collection.TaskAdded });              
            }
        }        
    }
}
