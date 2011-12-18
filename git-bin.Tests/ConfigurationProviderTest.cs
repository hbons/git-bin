using System;
using GitBin;
using NUnit.Framework;
using Moq;

namespace git_bin.Tests
{
    [TestFixture]
    public class ConfigurationProviderTest
    {
        private Mock<IGitConfigExecutor> _configExecutor;

        [SetUp]
        public void SetUp()
        {
            _configExecutor = new Mock<IGitConfigExecutor>();

            _configExecutor.Setup(x => x.GetString(ConfigurationProvider.S3KeyName)).Returns("a");
            _configExecutor.Setup(x => x.GetString(ConfigurationProvider.S3SecretKeyName)).Returns("a");
            _configExecutor.Setup(x => x.GetString(ConfigurationProvider.S3BucketName)).Returns("a");
        }

        [Test]
        public void ChunkSize_ValueIsPositive_GetsSet()
        {
            _configExecutor.Setup(x => x.GetLong(ConfigurationProvider.ChunkSizeName)).Returns(42);

            var target = new ConfigurationProvider(_configExecutor.Object);

            Assert.AreEqual(42, target.ChunkSize);
        }

        [Test]
        public void ChunkSize_ValueIsNegative_Throws()
        {
            _configExecutor.Setup(x => x.GetLong(ConfigurationProvider.ChunkSizeName)).Returns(-42);

            try
            {
                var target = new ConfigurationProvider(_configExecutor.Object);
            }
            catch (ಠ_ಠ)
            {
                Assert.Pass();
            }

            Assert.Fail("Exception not thrown for negative chunk size");
        }

        [Test]
        public void ChunkSize_ValueIsEmpty_SetToDefault()
        {
            _configExecutor.Setup(x => x.GetLong(ConfigurationProvider.ChunkSizeName)).Returns((int?)null);

            var target = new ConfigurationProvider(_configExecutor.Object);

            Assert.AreEqual(ConfigurationProvider.DefaultChunkSize, target.ChunkSize);
        }

        [Test]
        public void MaximumCacheSize_ValueIsPositive_GetsSet()
        {
            _configExecutor.Setup(x => x.GetLong(ConfigurationProvider.MaximumCacheSizeName)).Returns(42);

            var target = new ConfigurationProvider(_configExecutor.Object);

            Assert.AreEqual(42, target.MaximumCacheSize);
        }

        [Test]
        public void MaximumCacheSize_ValueIsNegative_Throws()
        {
            _configExecutor.Setup(x => x.GetLong(ConfigurationProvider.MaximumCacheSizeName)).Returns(-42);

            try
            {
                var target = new ConfigurationProvider(_configExecutor.Object);
            }
            catch (ಠ_ಠ)
            {
                Assert.Pass();
            }

            Assert.Fail("Exception not thrown for negative cache size");
        }

        [Test]
        public void MaximumCacheSize_ValueIsEmpty_SetToDefault()
        {
            _configExecutor.Setup(x => x.GetLong(ConfigurationProvider.MaximumCacheSizeName)).Returns((int?)null);

            var target = new ConfigurationProvider(_configExecutor.Object);

            Assert.AreEqual(ConfigurationProvider.DefaultMaximumCacheSize, target.MaximumCacheSize);
        }

        [Test]
        public void S3Key_HasValue_GetsSet()
        {
            string key = "i am a key";

            _configExecutor.Setup(x => x.GetString(ConfigurationProvider.S3KeyName)).Returns(key);

            var target = new ConfigurationProvider(_configExecutor.Object);

            Assert.AreEqual(key, target.S3Key);
        }

        [Test]
        public void S3Key_NoValue_Throws()
        {
            _configExecutor.Setup(x => x.GetString(ConfigurationProvider.S3KeyName)).Returns(string.Empty);

            try
            {
                var target = new ConfigurationProvider(_configExecutor.Object);
            }
            catch (ಠ_ಠ)
            {
                Assert.Pass();
            }

            Assert.Fail("Exception not thrown for s3key");
        }

        [Test]
        public void S3SecretKey_HasValue_GetsSet()
        {
            string key = "i am a key";

            _configExecutor.Setup(x => x.GetString(ConfigurationProvider.S3SecretKeyName)).Returns(key);

            var target = new ConfigurationProvider(_configExecutor.Object);

            Assert.AreEqual(key, target.S3SecretKey);
        }

        [Test]
        public void S3SecretKey_NoValue_Throws()
        {
            _configExecutor.Setup(x => x.GetString(ConfigurationProvider.S3SecretKeyName)).Returns(string.Empty);

            try
            {
                var target = new ConfigurationProvider(_configExecutor.Object);
            }
            catch (ಠ_ಠ)
            {
                Assert.Pass();
            }

            Assert.Fail("Exception not thrown for S3SecretKey");
        }

        [Test]
        public void S3Bucket_HasValue_GetsSet()
        {
            string key = "i am a key";

            _configExecutor.Setup(x => x.GetString(ConfigurationProvider.S3BucketName)).Returns(key);

            var target = new ConfigurationProvider(_configExecutor.Object);

            Assert.AreEqual(key, target.S3Bucket);
        }

        [Test]
        public void S3Bucket_NoValue_Throws()
        {
            _configExecutor.Setup(x => x.GetString(ConfigurationProvider.S3BucketName)).Returns(string.Empty);

            try
            {
                var target = new ConfigurationProvider(_configExecutor.Object);
            }
            catch (ಠ_ಠ)
            {
                Assert.Pass();
            }

            Assert.Fail("Exception not thrown for S3Bucket");
        }
    }
}