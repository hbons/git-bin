using System;

namespace GitBin
{
    public interface IConfigurationProvider
    {
        long ChunkSize { get; }
        long MaximumCacheSize { get; }
        string S3Key { get; }
        string S3SecretKey { get; }
        string S3Bucket { get; }
    }

    public class ConfigurationProvider : IConfigurationProvider
    {
        public const long DefaultChunkSize = 1024*1024;
        public const long DefaultMaximumCacheSize = long.MaxValue;

        public const string ChunkSizeName = "chunkSize";
        public const string MaximumCacheSizeName = "maxCacheSize";
        public const string S3KeyName = "s3key";
        public const string S3SecretKeyName = "s3secretKey";
        public const string S3BucketName = "s3bucket";

        private readonly IGitConfigExecutor _configExecutor;

        public long ChunkSize { get; private set; }
        public long MaximumCacheSize { get; private set; }
        public string S3Key { get; private set; }
        public string S3SecretKey { get; private set; }
        public string S3Bucket { get; private set; }

        public ConfigurationProvider(IGitConfigExecutor configExecutor)
        {
            _configExecutor = configExecutor;

            this.ChunkSize = GetLongValue(ChunkSizeName, DefaultChunkSize);
            this.MaximumCacheSize = GetLongValue(MaximumCacheSizeName, DefaultMaximumCacheSize);
            this.S3Key = GetStringValue(S3KeyName);
            this.S3SecretKey = GetStringValue(S3SecretKeyName);
            this.S3Bucket = GetStringValue(S3BucketName);
        }

        private long GetLongValue(string name, long defaultValue)
        {
            var rawValue = _configExecutor.GetLong(name);

            if (!rawValue.HasValue)
                return defaultValue;

            if (rawValue < 0)
                throw new ಠ_ಠ(name + " cannot be negative");

            return rawValue.Value;
        }

        private string GetStringValue(string name)
        {
            var rawValue = _configExecutor.GetString(name);

            if (string.IsNullOrEmpty(rawValue))
                throw new ಠ_ಠ(name + " must be set");

            return rawValue;
        }
    }
}