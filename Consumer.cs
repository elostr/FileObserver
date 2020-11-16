using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileObserver.Contracts;
using NLog;

namespace FileObserver
{
    public class Consumer
    {
        private readonly IConsumerCollection _collection;

        private readonly IFileWorker _worker;

        private readonly IResultsWriter _writer;

        private readonly CancellationTokenSource _ctSource;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
                    ProcessTask(token, task);
                }

                WaitHandle.WaitAny(new[] { token.WaitHandle, _collection.TaskAdded });              
            }
        }

        private void ProcessTask(CancellationToken token, string task)
        {
            var attempts = 0;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var charCount = _worker.Work(task);
                    _writer.Write(task, charCount);
                    return;
                }
                catch (IOException ex)
                {
                    _logger.Error(ex, $"Во время чтения файла {task} произошла ошибка.");
                    attempts++;
                    if (attempts >= 3)
                    {
                        _logger.Error($"Файл {task} не обработан.");
                    }
                    else
                    {
                        Task.Delay(10000, token);
                    }
                }
            }
        }
    }
}
