using GitBin;
using GitBin.Commands;
using GitBin.Remotes;
using Moq;
using NUnit.Framework;

namespace git_bin.Tests.Commands
{
    [TestFixture]
    public class StatusCommandTest
    {
        private Mock<ICacheManager> _cacheManager;
        private Mock<IRemote> _remote;

        [SetUp]
        public void SetUp()
        {
            _cacheManager = new Mock<ICacheManager>();
            _remote = new Mock<IRemote>();
        }

        [Test]
        public void GetHumanReadableSize_LessThanOrEqualTo1K_StringIsBytes()
        {
            var zero = StatusCommand.GetHumanReadableSize(0);
            var fifty = StatusCommand.GetHumanReadableSize(50);
            var kilobyte = StatusCommand.GetHumanReadableSize(1024);

            Assert.AreEqual("0B", zero);
            Assert.AreEqual("50B", fifty);
            Assert.AreEqual("1k", kilobyte);
        }

        [Test]
        public void GetHumanReadableSize_Between1KAnd1MInclusive_StringIsKB()
        {
            var ninePointEight = StatusCommand.GetHumanReadableSize(10035);
            var megabyte = StatusCommand.GetHumanReadableSize(1024*1024);

            Assert.AreEqual("9.8k", ninePointEight);
            Assert.AreEqual("1M", megabyte);
        }

        [Test]
        public void GetHumanReadableSize_Between1Mand1GInclusive_StringIsMB()
        {
            var fourPointTwo = StatusCommand.GetHumanReadableSize(4404019);
            var gigabyte = StatusCommand.GetHumanReadableSize(1024*1024*1024);

            Assert.AreEqual("4.2M", fourPointTwo);
            Assert.AreEqual("1G", gigabyte);
        }
    }
}