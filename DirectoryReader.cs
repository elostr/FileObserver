using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileObserver
{
    public class DirectoryReader
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        
        private string _path;

        public DirectoryReader(string path)
        {
            PathVerifier.Verify(path);
            if (Directory.Exists(path))
            {
                _path = path;
            }
            else
            {
                throw new ArgumentException("Папки с таким именем не существует.");
            }
        }

        public void Read()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_path);
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo fileInfo in files)
            {
                readFileAsyns(fileInfo.FullName, fileInfo.Name);
            }
        }

        public async void readFileAsyns(string filePath, string fileName)
        {
            await Task.Run(()=> countCharacter(filePath, fileName));
        }

        private void countCharacter(string filePath, string fileName)
        {
            int charCount = CharacterCounter.Count(filePath);
            _logger.Info("Файл: {0}, количество символов {1}", fileName, charCount);           
        }
    }
}
