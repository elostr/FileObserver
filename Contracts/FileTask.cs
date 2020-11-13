namespace FileObserver.Contracts
{
    public struct FileTask
    {
        public FileTask(string path, string fileName)
        {
            Path = path;
            Name = fileName;
        }

        public string Path { get; }

        public string Name { get; }
    }
}
