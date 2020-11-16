
namespace FileObserver.Contracts
{
    /// <summary>
    /// Интерфейс шины данных для producer
    /// </summary>
    public interface IProducerCollection
    {
         void Add(string filePath);
    }
}
