using System;
using System.IO;
using System.Linq;

namespace FileObserver
{
    public static class CharacterCounter
    {
        public static int Count(string filePath)
        {
            verify(filePath);
            return File.ReadAllText(filePath).Count(c => !Char.IsControl(c) && !Char.IsWhiteSpace(c));
        }

        private static void verify(string filePath)
        {
            PathVerifier.Verify(filePath);
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("Файла с именем: {0} не существует.", filePath);
            }
        }
    }
}
