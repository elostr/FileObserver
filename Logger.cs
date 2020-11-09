using NLog;

namespace FileObserver
{
    public static class Logger
    {
        private static NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        public static void WriteCountOfCharacters(string fileName, long count)
        {
            _logger.Info("Файл: {0}, количество символов {1}", fileName, count);
        }
    }
}
