using FileObserver.Contracts;
using NLog;

namespace FileObserver
{
    public class ResultsWriter : IResultsWriter
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Write(string fileName, long count)
        {
            _logger.Info($"{fileName}: {count}");
        }
    }
}
