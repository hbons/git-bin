using System;
using GitBin;
using NUnit.Framework;
using Moq;

namespace git_bin.Tests
{
    [TestFixture]
    public class ConfigurationProviderTest
    {
        private Mock<IGitExecutor> _gitExecutor;

        [SetUp]
        public void SetUp()
        {
            _gitExecutor = new Mock<IGitExecutor>();

            _gitExecutor.Setup(x => x.GetString(GetConfigArgumentString(ConfigurationProvider.S3KeyName))).Returns("a");
            _gitExecutor.Setup(x => x.GetString(GetConfigArgumentString(ConfigurationProvider.S3SecretKeyName))).Returns("a");
            _gitExecutor.Setup(x => x.GetString(GetConfigArgumentString(ConfigurationProvider.S3BucketName))).Returns("a");
            _gitExecutor.Setup(x => x.GetString("rev-parse --git-dir")).Returns("a");
        }

        private string GetConfigArgumentString(string keyName)
        {
            return "config " + ConfigurationProvider.SectionName + '.' + keyName;
        }

        private string GetConfigArgumentStringForInt(string keyName)
        {
            return "config --int " + ConfigurationProvider.SectionName + '.' + keyName;
        }

        [Test]
        public void ChunkSize_ValueIsPositive_GetsSet()
        {
            _gitExecutor.Setup(x => x.GetLong(GetConfigArgumentStringForInt(ConfigurationProvider.ChunkSizeName))).Returns(42);

            var target = new ConfigurationProvider(_gitExecutor.Object);

            Assert.AreEqual(42, target.ChunkSize);
        }

        [Test]
        public void ChunkSize_ValueIsNegative_Throws()
        {
            _gitExecutor.Setup(x => x.GetLong(GetConfigArgumentStringForInt(ConfigurationProvider.ChunkSizeName))).Returns(-42);

            try
            {
                var target = new ConfigurationProvider(_gitExecutor.Object);
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
            _gitExecutor.Setup(x => x.GetLong(GetConfigArgumentStringForInt(ConfigurationProvider.ChunkSizeName))).Returns((int?)null);

            var target = new ConfigurationProvider(_gitExecutor.Object);

            Assert.AreEqual(ConfigurationProvider.DefaultChunkSize, target.ChunkSize);
        }

        [Test]
        public void MaximumCacheSize_ValueIsPositive_GetsSet()
        {
            _gitExecutor.Setup(x => x.GetLong(GetConfigArgumentStringForInt(ConfigurationProvider.MaximumCacheSizeName))).Returns(42);

            var target = new ConfigurationProvider(_gitExecutor.Object);

            Assert.AreEqual(42, target.MaximumCacheSize);
        }

        [Test]
        public void MaximumCacheSize_ValueIsNegative_Throws()
        {
            _gitExecutor.Setup(x => x.GetLong(GetConfigArgumentStringForInt(ConfigurationProvider.MaximumCacheSizeName))).Returns(-42);

            try
            {
                var target = new ConfigurationProvider(_gitExecutor.Object);
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
            _gitExecutor.Setup(x => x.GetLong(GetConfigArgumentStringForInt(ConfigurationProvider.MaximumCacheSizeName))).Returns((int?)null);

            var target = new ConfigurationProvider(_gitExecutor.Object);

            Assert.AreEqual(ConfigurationProvider.DefaultMaximumCacheSize, target.MaximumCacheSize);
        }

        [Test]
        public void S3Key_HasValue_GetsSet()
        {
            string key = "i am a key";

            _gitExecutor.Setup(x => x.GetString(GetConfigArgumentString(ConfigurationProvider.S3KeyName))).Returns(key);

            var target = new ConfigurationProvider(_gitExecutor.Object);

            Assert.AreEqual(key, target.S3Key);
        }

        [Test]
        public void S3Key_NoValue_Throws()
        {
            _gitExecutor.Setup(x => x.GetString(GetConfigArgumentString(ConfigurationProvider.S3KeyName))).Returns(string.Empty);

            try
            {
                var target = new ConfigurationProvider(_gitExecutor.Object);
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

            _gitExecutor.Setup(x => x.GetString(GetConfigArgumentString(ConfigurationProvider.S3SecretKeyName))).Returns(key);

            var target = new ConfigurationProvider(_gitExecutor.Object);

            Assert.AreEqual(key, target.S3SecretKey);
        }

        [Test]
        public void S3SecretKey_NoValue_Throws()
        {
            _gitExecutor.Setup(x => x.GetString(GetConfigArgumentString(ConfigurationProvider.S3SecretKeyName))).Returns(string.Empty);

            try
            {
                var target = new ConfigurationProvider(_gitExecutor.Object);
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

            _gitExecutor.Setup(x => x.GetString(GetConfigArgumentString(ConfigurationProvider.S3BucketName))).Returns(key);

            var target = new ConfigurationProvider(_gitExecutor.Object);

            Assert.AreEqual(key, target.S3Bucket);
        }

        [Test]
        public void S3Bucket_NoValue_Throws()
        {
            _gitExecutor.Setup(x => x.GetString(GetConfigArgumentString(ConfigurationProvider.S3BucketName))).Returns(string.Empty);

            try
            {
                var target = new ConfigurationProvider(_gitExecutor.Object);
            }
            catch (ಠ_ಠ)
            {
                Assert.Pass();
            }

            Assert.Fail("Exception not thrown for S3Bucket");
        }

        [Test]
        public void CacheDirectory_HasValue_GetsSet()
        {
            string dir = "directory";

            _gitExecutor.Setup(x => x.GetString("rev-parse --git-dir")).Returns(dir);

            var target = new ConfigurationProvider(_gitExecutor.Object);

            Assert.AreEqual(dir, target.CacheDirectory);
        }

        [Test]
        public void CacheDirectory_NoValue_Throws()
        {
            _gitExecutor.Setup(x => x.GetString("rev-parse --git-dir")).Returns(string.Empty);

            try
            {
                var target = new ConfigurationProvider(_gitExecutor.Object);
            }
            catch (ಠ_ಠ)
            {
                Assert.Pass();
            }

            Assert.Fail("Exception not thrown for CacheDir");
        }
    }
}