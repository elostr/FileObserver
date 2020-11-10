using System;
using System.IO;
using System.Threading;
using FileObserver.Contracts;

namespace FileObserver
{
    class Program
    {       
        private const string _path = @"d:\StudyingProgects\FileObserver\Data\";

        static void Main(string[] args)
        {
            Verify(_path);

            FileTaskCollection taskCollection = new FileTaskCollection();
            IResultsWriter writer = new ResultsWriter();
            IFileWorker worker = new FileWorker(writer);

            Producer watcher = null;
            try
            {
                CancellationTokenSource source = new CancellationTokenSource();

                watcher = new Producer(_path, taskCollection);
                watcher.Start();

                Consumer[] consumers = new Consumer[4];
                for (int i = 0; i < consumers.Length; i++)
                {
                    consumers[i] = new Consumer(taskCollection, worker);
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

            if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                throw new ArgumentException("Путь содержит недопустимые символы.");
            }
        }
    }
}
