using System;
using System.IO;
using NUnit.Framework;

namespace FileObserver.Tests
{
    [TestFixture]
    public class FileWorkerTest
    {
        private string _fileName;

        [SetUp]
        public void SetUp()
        {
            _fileName = $"{Guid.NewGuid()}.txt";
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }
        }

        [Test]
        public void WithWhiteSpacesTest()
        {
            string createText = "Hy all"; 
            File.WriteAllText(_fileName, createText);
            
            FileWorker worker = new FileWorker();
            Assert.AreEqual(worker.Work(_fileName),5 );
            
        }

        [Test]
        public void WithNewLineTest()
        {
            string createText = "Hello " + Environment.NewLine + "world"; 
            File.WriteAllText(_fileName, createText);
            
            FileWorker worker = new FileWorker();
            Assert.AreEqual(worker.Work(_fileName),10 );
        }

        [Test]
        public void WithoutWhiteSpacesTest()
        {
            string createText = "Hello"; 
            File.WriteAllText(_fileName, createText);
            
            FileWorker worker = new FileWorker();
            Assert.AreEqual(worker.Work(_fileName),5 );
        }
    }
}