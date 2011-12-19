using System;
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
        public void Ctor_OneArgument_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => 
                new CleanCommand(
                    _configurationProvider.Object,
                    _cacheManager.Object,
                    new[] {"filename"}));
        }

        [Test]
        public void Ctor_WrongNumberOfArguments_Throws()
        {
            Assert.Throws<ArgumentException>(() => 
                new CleanCommand(
                    _configurationProvider.Object,
                    _cacheManager.Object,
                    new string[0]));

            Assert.Throws<ArgumentException>(() =>
                new CleanCommand(
                    _configurationProvider.Object,
                    _cacheManager.Object,
                    new[] {"a", "b", "c"}));
        }
    }
}