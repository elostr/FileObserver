using System.IO;
using System.Linq;
using FileObserver.Contracts;

namespace FileObserver
{
    public class FileWorker : IFileWorker
    {
        /// <summary>
        /// Для примера: просто считаем количество символов.
        /// </summary>
        public int Work(string path)
        {
            return File.ReadAllText(path).Count(c => !char.IsControl(c) && !char.IsWhiteSpace(c));
        }
    }
}
