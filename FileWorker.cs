using System;
using System.IO;
using System.Linq;
using FileObserver.Contracts;

namespace FileObserver
{
    /// <summary>
    /// Что-то делает с полученным файлом.
    /// </summary>
    public class FileWorker : IFileWorker
    {
        /// <summary>
        /// Считает количество символов в файле.
        /// </summary>
        public int Work(string path)
        {
            return File.ReadAllText(path).Count(c => !Char.IsControl(c) && !Char.IsWhiteSpace(c));
        }
    }
}
