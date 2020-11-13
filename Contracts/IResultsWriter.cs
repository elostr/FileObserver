
namespace FileObserver.Contracts
{
    public interface IResultsWriter
    {
        void Write(string filePath, long count);
    }
}
