using NLog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileObserver
{
    class Program
    {       
        private const string Path = @"d:\StudyingProgects\FileObserver\Data\";        

        static void Main(string[] args)
        {
            SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(4);

            DirectoryWatcher watcher = null;
            DirectoryReader reader = null;
            try
            {
                reader = new DirectoryReader(Path, _semaphoreSlim);
                reader.Read();

                watcher = new DirectoryWatcher(Path, _semaphoreSlim);
                watcher.Start();
                Console.Read();
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
    }
}
