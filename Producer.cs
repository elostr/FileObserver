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
            init();
            if (_watcher != null)
            {
                return;
            }

            _watcher = new FileSystemWatcher(_path);
            _watcher.NotifyFilter = NotifyFilters.LastWrite
                                 | NotifyFilters.FileName;

            _watcher.Changed += watcher_Changed;
            _watcher.Created += watcher_Changed;
            _watcher.Deleted += watcher_Changed;
            _watcher.Renamed += watcher_Renamed;

            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
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

        private void init()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_path);
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo fileInfo in files)
            {
                _collection.Add(new FileTask(fileInfo.FullName));
            }
        }

        private void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            _collection.Add(new FileTask(e.FullPath));
        }

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            _collection.Add(new FileTask(e.FullPath));
        }
    }
}
