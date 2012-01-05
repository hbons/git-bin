using GitBin;
using GitBin.Commands;
using GitBin.Remotes;
using Moq;
using NUnit.Framework;

namespace git_bin.Tests
{
    [TestFixture]
    public class GitBinFileInfoUtilsTest
    {
        [Test]
        public void GetHumanReadableSize_LessThanOrEqualTo1K_StringIsBytes()
        {
            var zero = GitBinFileInfoUtils.GetHumanReadableSize(0);
            var fifty = GitBinFileInfoUtils.GetHumanReadableSize(50);
            var kilobyte = GitBinFileInfoUtils.GetHumanReadableSize(1024);

            Assert.AreEqual("0B", zero);
            Assert.AreEqual("50B", fifty);
            Assert.AreEqual("1k", kilobyte);
        }

        [Test]
        public void GetHumanReadableSize_Between1KAnd1MInclusive_StringIsKB()
        {
            var ninePointEight = GitBinFileInfoUtils.GetHumanReadableSize(10035);
            var megabyte = GitBinFileInfoUtils.GetHumanReadableSize(1024 * 1024);

            Assert.AreEqual("9.8k", ninePointEight);
            Assert.AreEqual("1M", megabyte);
        }

        [Test]
        public void GetHumanReadableSize_Between1Mand1GInclusive_StringIsMB()
        {
            var fourPointTwo = GitBinFileInfoUtils.GetHumanReadableSize(4404019);
            var gigabyte = GitBinFileInfoUtils.GetHumanReadableSize(1024 * 1024 * 1024);

            Assert.AreEqual("4.2M", fourPointTwo);
            Assert.AreEqual("1G", gigabyte);
        }
    }
}