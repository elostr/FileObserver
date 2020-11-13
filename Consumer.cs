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

        public Consumer(IConsumerCollection collection, IFileWorker worker, IResultsWriter writer)
        {
            _collection = collection;
            _worker = worker;
            _writer = writer;
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
                    int charCount = _worker.Work(task.Path);
                    _writer.Write(task.Name, charCount);
                }

                WaitHandle.WaitAny(waitHandles: new[] { token.WaitHandle, _collection.TaskAdded });              
            }
        }        
    }
}
