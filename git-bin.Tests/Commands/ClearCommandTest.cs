using GitBin;
using GitBin.Commands;
using Moq;
using NUnit.Framework;

namespace git_bin.Tests.Commands
{
    [TestFixture]
    public class ClearCommandTest
    {
        private Mock<ICacheManager> _cacheManager;

        [SetUp]
        public void SetUp()
        {
            _cacheManager = new Mock<ICacheManager>(MockBehavior.Strict);
        }

        [Test]
        public void Execute_Force_CallsClearCache()
        {
            _cacheManager.Setup(x => x.ClearCache()).Verifiable();

            var target = new ClearCommand(_cacheManager.Object, new[] {"-f"});
            target.Execute();

            _cacheManager.Verify();
        }

        [Test]
        public void Execute_DryRun_DoesNotClearCache()
        {
            // mock is strict, so calling ClearCache will throw
            _cacheManager.Setup(x => x.ListFiles()).Returns(new GitBinFileInfo[0]);

            var target = new ClearCommand(_cacheManager.Object, new[] { "-n" });
            target.Execute();

            Assert.Pass();
        }
    }
}