using System.Threading;
using FileObserver.Contracts;
using Moq;
using NUnit.Framework;

namespace FileObserver.Tests
{
    [TestFixture]
    public class ConsumerTest
    {
        private Mock<IResultsWriter> _mockWriter;
        private Mock<IFileWorker> _mockWorker;
        private FileTaskCollection _collection;

        private AutoResetEvent _callEvent;

        private const string FilePath = "SomeFile";

        private Consumer _consumer;

        [SetUp]
        public void SetUp()
        {
            _callEvent = new AutoResetEvent(false);

            _mockWriter = new Mock<IResultsWriter>();
            _mockWriter.Setup(o => o.Write(FilePath, 10)).Callback(() => _callEvent.Set());

            _mockWorker = new Mock<IFileWorker>();
            _mockWorker.Setup(m => m.Work(FilePath)).Returns(10);

            _collection = new FileTaskCollection();
            _consumer = new Consumer(_collection, _mockWorker.Object, _mockWriter.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _consumer.Stop();
        }

        [Test]
        public void StopWorkTest()
        {
            _consumer.Start();
            
            _consumer.Stop();
            _collection.Add(FilePath);

            Assert.IsFalse(_callEvent.WaitOne(1000));
            _mockWorker.Verify( m=>m.Work(It.IsAny<string>()), Times.Never);
            _mockWriter.Verify(m => m.Write(It.IsAny<string>(), It.IsAny<long>()), Times.Never);
        }

        [Test]
        public void InitializationTest()
        {
            _collection.Add(FilePath);
            _consumer.Start();
            
            Assert.IsTrue(_callEvent.WaitOne(1000));
            _mockWorker.Verify( m=>m.Work(FilePath), Times.Once);
            _mockWriter.Verify(m => m.Write(FilePath, It.IsAny<long>()), Times.Once);
        }

        [Test]
        public void EmptyCollectionTest()
        {
            _consumer.Start();

            Assert.IsFalse(_callEvent.WaitOne(1000));
            _mockWorker.Verify(m => m.Work(FilePath), Times.Never);
            _mockWriter.Verify(m => m.Write(FilePath, It.IsAny<long>()), Times.Never);
        }

        [Test]
        public void AddNewTaskTest()
        {
            _consumer.Start();

            Assert.IsFalse(_callEvent.WaitOne(1000));
            _mockWorker.Verify(m => m.Work(It.IsAny<string>()), Times.Never);
            _mockWriter.Verify(m => m.Write(It.IsAny<string>(), It.IsAny<long>()), Times.Never);

            _collection.Add(FilePath);
            
            Assert.IsTrue(_callEvent.WaitOne(1000));
            _mockWorker.Verify( m=>m.Work(FilePath), Times.Once);
            _mockWriter.Verify(m => m.Write(FilePath, It.IsAny<long>()), Times.Once);
        }
    }
}
