using System;
using System.IO;
using System.Linq;
using FileObserver.Contracts;

namespace FileObserver
{
    public class FileWorker : IFileWorker
    {
        private readonly IResultsWriter _writer;

        public FileWorker(IResultsWriter writer)
        {
            _writer = writer;
        }

        public void Work(FileTask fileTask)
        {
            int charCount = File.ReadAllText(fileTask.Path).Count(c => !Char.IsControl(c) && !Char.IsWhiteSpace(c));
            _writer.Write(fileTask.Name, charCount);
        }
    }
}
