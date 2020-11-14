using System;
using System.IO;

namespace FileObserver
{
    internal class Program
    {       
        private const string Path = @"d:\StudyingProgects\FileObserver\Data\";

        private static void Main()
        {
            VerifyPath(Path);

            var taskCollection = new FileTaskCollection();
            var writer = new ResultsWriter();
            var worker = new FileWorker();

            var consumers = new Consumer[4];
            Producer producer = null;
            try
            {
                producer = new Producer(Path, taskCollection);
                producer.Start();

                for (var i = 0; i < consumers.Length; i++)
                {
                    consumers[i] = new Consumer(taskCollection, worker, writer);
                    consumers[i].Start();
                }

                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                foreach (var t in consumers)
                {
                    t?.Stop();
                }
                producer?.Stop();
            }
        }

        public static void VerifyPath(string path)
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
