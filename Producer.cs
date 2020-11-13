using System.IO;
using FileObserver.Contracts;

namespace FileObserver
{
    public class Producer 
    {
        private FileSystemWatcher _watcher;

        private readonly IProducerCollection _collection;
        private readonly string _path;

        public Producer(string path, IProducerCollection collection)
        {
            _path = path;
            _collection = collection;
        }

        public void Start()
        {
            Init();
            if (_watcher != null)
            {
                return;
            }

            _watcher = new FileSystemWatcher(_path)
            {
                NotifyFilter = NotifyFilters.LastWrite
                               | NotifyFilters.FileName
            };

            _watcher.Changed += Watcher_Changed;
            _watcher.Created += Watcher_Changed;
            _watcher.Deleted += Watcher_Changed;
            _watcher.Renamed += Watcher_Renamed;

            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            if (_watcher == null)
            {
                return;
            }

            _watcher.EnableRaisingEvents = false;

            _watcher.Changed -= Watcher_Changed;
            _watcher.Created -= Watcher_Changed;
            _watcher.Deleted -= Watcher_Changed;
            _watcher.Renamed -= Watcher_Renamed;

            _watcher.Dispose();
        }

        private void Init()
        {
            var directoryInfo = new DirectoryInfo(_path);
            var files = directoryInfo.GetFiles();
            foreach (var fileInfo in files)
            {
                _collection.Add(fileInfo.FullName);
            }
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            _collection.Add(e.FullPath);
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            _collection.Add(e.FullPath);
        }
    }
}
