namespace FileObserver
{
    public struct FileTask
    {
        private readonly string _path;

        private readonly string _fileName;
        public FileTask(string path, string fileName)
        {
            _path = path;
            _fileName = fileName;
        }

        public string Path => _path;
        public string Name => _fileName;
    }
}
