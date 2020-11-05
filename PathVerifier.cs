using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileObserver
{
    public static class PathVerifier
    {
        public static void Verify(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Путь не задан.");
            }

            if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                throw new ArgumentException("Путь содержит недопустимые символы.");
            }            
        }
    }
}
