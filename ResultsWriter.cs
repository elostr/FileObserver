using FileObserver.Contracts;
using NLog;

namespace FileObserver
{
    public class ResultsWriter : IResultsWriter
    {
        private readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        public void Write(string fileName, long count)
        {
            _logger.Info($"Файл: {fileName}, количество символов {count}");
        }
    }
}
