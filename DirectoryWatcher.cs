using NLog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileObserver
{
    public class DirectoryWatcher
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private FileSystemWatcher _watcher;

        private object _locker = new Object();

        private string _path;

        public DirectoryWatcher(string path)
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
        }

        public void Start()
        {
            lock(_locker)
            {
                if (_watcher != null)
                {
                    return;
                }

                _watcher = new FileSystemWatcher(_path);
                _watcher.NotifyFilter = NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.Size;

                _watcher.Changed += watcher_Changed;
                _watcher.Created += watcher_Changed;
                _watcher.Deleted += watcher_Changed;
                _watcher.Renamed += watcher_Renamed;

                _watcher.EnableRaisingEvents = true;
            } 
        }

        public void Stop()
        {
            lock (_locker)
            {
                if (_watcher == null)
                {
                    return;
                }

                _watcher.EnableRaisingEvents = false;

                _watcher.Changed -= watcher_Changed;
                _watcher.Created -= watcher_Changed;
                _watcher.Deleted -= watcher_Changed;
                _watcher.Renamed -= watcher_Renamed;

                _watcher.Dispose();
            }
        }

        private async void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            try
            {
                await Task.Run(() => watchChanges(e.FullPath, e.Name));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                await Task.Run(() => watchChanges(e.FullPath, e.Name));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void watchChanges(string path, string fileName)
        {
            int charCount = CharacterCounter.Count(path);
            _logger.Info("Файл: {0}, количество символов {1}", fileName, charCount);
        }
    }
}
