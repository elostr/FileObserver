using System;
using NLog;

namespace FileObserver
{
    internal class Program
    {       
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                _logger.Error("Способ запуска: FileObserver.exe <путь к папке с файлами>");
                return;
            }

            var service = new FileWatcherService(args[0], 4);

            try
            {
                _logger.Info("Нажмите любую клавишу для остановки работы");
                service.Start();

                Console.ReadKey();

                service.Stop();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Произошла ошибка во время работы приложения.");
            }
        }
    }
}
