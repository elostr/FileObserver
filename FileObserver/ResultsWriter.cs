using FileObserver.Contracts;
using NLog;

namespace FileObserver
{
    public class ResultsWriter : IResultsWriter
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Write(string filePath, long count)
        {
            _logger.Info($"{filePath}: {count}");
        }
    }
}
