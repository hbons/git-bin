using GitBin;
using GitBin.Commands;
using Moq;
using NUnit.Framework;

namespace git_bin.Tests.Commands
{
    [TestFixture]
    public class CleanCommandTest
    {
        private Mock<IConfigurationProvider> _configurationProvider;
        private Mock<ICacheManager> _cacheManager;

        [SetUp]
        public void SetUp()
        {
            _configurationProvider = new Mock<IConfigurationProvider>();
            _cacheManager = new Mock<ICacheManager>();
        }

        [Test]
        public void Ctor_OneArgument_CanExecuteIsTrue()
        {
            var oneArgument = new CleanCommand(
                _configurationProvider.Object,
                _cacheManager.Object,
                new[] {"filename"});

            Assert.IsTrue(oneArgument.CanExecute);
        }

        [Test]
        public void Ctor_WrongNumberOfArguments_CanExecuteIsFalse()
        {
            var noArguments = new CleanCommand(
                _configurationProvider.Object,
                _cacheManager.Object,
                new string[0]);

            var moreThanOneArgument = new CleanCommand(
                _configurationProvider.Object,
                _cacheManager.Object,
                new[] {"a", "b", "c"});

            Assert.IsFalse(noArguments.CanExecute);
            Assert.IsFalse(moreThanOneArgument.CanExecute);
        }
    }
}