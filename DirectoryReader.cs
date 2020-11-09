using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileObserver
{
    public class DirectoryReader
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        
        private string _path;

        private SemaphoreSlim _semaphoreSlim;

        public DirectoryReader(string path, SemaphoreSlim semaphoreSlim)
        {
            PathVerifier.Verify(path);
            if (Directory.Exists(path))
            {
                _path = path;
            }
            else
            {
                throw new ArgumentException("Папки с таким именем не существует.");
            }
            _semaphoreSlim = semaphoreSlim;           
        }

        public async void Read()
        {
            var allTasks = new List<Task>();

            DirectoryInfo directoryInfo = new DirectoryInfo(_path);
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo fileInfo in files)
            {
                await _semaphoreSlim.WaitAsync();
                allTasks.Add(
                    Task.Run(() =>
                    {
                        try
                        {
                            int charCount = CharacterCounter.Count(fileInfo.FullName);
                            _logger.Info("Файл: {0}, количество символов {1}", fileInfo.Name, charCount);
                        }
                        finally
                        {
                            _semaphoreSlim.Release();
                        }
                    }));                
            }

            await Task.WhenAll(allTasks);
        }
    }
}
