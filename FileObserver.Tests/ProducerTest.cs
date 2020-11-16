using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace FileObserver.Tests
{
    [TestFixture]
    public class ProducerTest
    {
        private FileTaskCollection _collection;
        private string _directoryPath;
        private Producer _producer;

        [SetUp]
        public void SetUp()
        {
            _collection = new FileTaskCollection();
            _directoryPath = CreateDirectory();
            _producer = new Producer(_directoryPath, _collection); 
        }

        [TearDown]
        public void TearDown()
        {
            _producer.Stop();
            if (Directory.Exists(_directoryPath))
            {
                Directory.Delete(_directoryPath, true);
            }
        }

        [Test]
        public void AddNewItemTest()
        {
            _producer.Start();
            Assert.IsFalse(_collection.TaskAdded.WaitOne(1));

            CreateFile(_directoryPath, "New item test");
            Assert.IsTrue(_collection.TaskAdded.WaitOne(TimeSpan.FromSeconds(10)));
        }

        [Test]
        public void InitializationTest()
        {
            var tasks = new List<string>
            {
                CreateFile(_directoryPath, "Initialization test 1"),
                CreateFile(_directoryPath, "Initialization test 2"),
                CreateFile(_directoryPath, "Initialization test 3")
            };

            _producer.Start();

            var expectedTasks = new List<string>();

            while (_collection.TryTake(out var task))
            {
                expectedTasks.Add(task);
            }

            Assert.AreEqual(expectedTasks.Count, tasks.Count);
            foreach (var fileTask in tasks)
            {
                CollectionAssert.Contains(expectedTasks, fileTask);
            }
        }

        
        private string CreateFile(string folderPath, string content)
        {
            var fileName = $"{Guid.NewGuid()}.txt";
            var filePath = Path.Combine(folderPath,fileName);
            File.WriteAllText(filePath, content);
            return filePath;
        }

        private string CreateDirectory()
        {
            var info = Directory.CreateDirectory(Guid.NewGuid().ToString());
            return info.FullName;
        }
    }
}
