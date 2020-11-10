namespace FileObserver
{
    public struct FileTask
    {
        private string _path;

        public FileTask(string path)
        {
            _path = path;
        }

        public string Path => _path;
    }
}
