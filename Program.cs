using System;
using System.Threading;
using FileObserver.Contracts;

namespace FileObserver
{
    class Program
    {       
        private const string Path = @"d:\StudyingProgects\FileObserver\Data\";

        static void Main()
        {
            Verify(Path);

            FileTaskCollection taskCollection = new FileTaskCollection();
            IResultsWriter writer = new ResultsWriter();
            IFileWorker worker = new FileWorker();

            Producer watcher = null;
            try
            {
                CancellationTokenSource source = new CancellationTokenSource();

                watcher = new Producer(Path, taskCollection);
                watcher.Start();

                Consumer[] consumers = new Consumer[4];
                for (int i = 0; i < consumers.Length; i++)
                {
                    consumers[i] = new Consumer(taskCollection, worker, writer);
                    consumers[i].Start(source.Token);
                }

                Console.Read();
                source.Cancel();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (watcher != null)
                {
                    watcher.Stop();
                }
            }
        }

        public static void Verify(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Путь не задан.");
            }

            if (path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
            {
                throw new ArgumentException("Путь содержит недопустимые символы.");
            }
        }
    }
}
