using System;
using System.Collections.Generic;
using System.IO;

namespace FileObserver
{
    public class FileWatcherService
    {
        private readonly string _directoryPath;
        private readonly int _threadCount;

        private Producer _producer;
        private IList<Consumer> _consumers;

        public FileWatcherService(string directoryPath, int threadCount)
        {
            VerifyPath(directoryPath);

            _directoryPath = directoryPath;
            _threadCount = threadCount;
        }

        public void Start()
        {
            var taskCollection = new FileTaskCollection();
            var writer = new ResultsWriter();
            var worker = new FileWorker();

            _producer = new Producer(_directoryPath, taskCollection);
            _producer.Start();

            _consumers = new List<Consumer>(_threadCount);
            for (var i = 0; i < _threadCount; i++)
            {
                var consumer = new Consumer(taskCollection, worker, writer);
                consumer.Start();
                _consumers.Add(consumer);
            }
        }

        public void Stop()
        {
            if (_producer != null)
            {
                _producer.Stop();
                _producer = null;
            }

            if (_consumers != null)
            {
                foreach (var consumer in _consumers)
                {
                    consumer.Stop();
                }

                _consumers = null;
            }
        }

        private static void VerifyPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Путь не задан.");
            }

            if (path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
            {
                throw new ArgumentException("Путь содержит недопустимые символы.");
            }

            if (!Directory.Exists(path))
            {
                throw  new ArgumentException("Заданный путь не существует.");
            }
        }
    }
}