using NLog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileObserver
{
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private const string Path = @"d:\StudyingProgects\FileObserver\Data\";

        static void Main(string[] args)
        {
            DirectoryWatcher watcher = null;
            DirectoryReader reader = null;
            try
            {
                reader = new DirectoryReader(Path);
                reader.Read();

                watcher = new DirectoryWatcher(Path);
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
