
namespace FileObserver.Contracts
{
    public interface IFileWorker
    {
        /// <summary>
        /// Cчитает количество символов в фвйле.
        /// </summary>
        int Work(string path);
    }
}
