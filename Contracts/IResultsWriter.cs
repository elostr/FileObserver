
namespace FileObserver.Contracts
{
    public interface IResultsWriter
    {
        void Write(string fileName, long count);
    }
}
