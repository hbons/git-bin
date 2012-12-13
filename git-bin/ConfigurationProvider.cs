using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace GitBin
{
    public interface IConfigurationProvider
    {
        long ChunkSize { get; }
        long MaximumCacheSize { get; }
        string CacheDirectory { get; }
        Hashtable Settings { get; }
    }

    public class ConfigurationProvider : IConfigurationProvider
    {
        public const long DefaultChunkSize = 1024*1024;
        public const long DefaultMaximumCacheSize = long.MaxValue;

        public const string DirectoryName = "git-bin";
        public const string SectionName = "git-bin";
        public const string ChunkSizeName = "chunkSize";
        public const string MaximumCacheSizeName = "maxCacheSize";


        public long ChunkSize { get; private set; }
        public long MaximumCacheSize { get; private set; }
        public string CacheDirectory { get; private set; }

        private readonly IGitExecutor _gitExecutor;
        private Hashtable _settings;


        public Hashtable Settings {
            get {
                if (_settings == null) {
                    _settings    = new Hashtable();
                    var rawValue = _gitExecutor.GetString("config --get-regexp " + SectionName + ".*");
                    Regex regex  = new Regex(SectionName + "\\.(.+) (.+)");
                    
                    foreach (Match match in regex.Matches (rawValue)) {
                        string key = match.Groups[1].ToString();
                        string val = match.Groups[2].ToString();

                        _settings.Add(key, val);
                    }
                }
                
                return _settings;
            }
        }


        public ConfigurationProvider(IGitExecutor gitExecutor)
        {
            _gitExecutor = gitExecutor;

            this.ChunkSize = GetLongValue(ChunkSizeName, DefaultChunkSize);
            this.MaximumCacheSize = GetLongValue(MaximumCacheSizeName, DefaultMaximumCacheSize);
            this.CacheDirectory = GetCacheDirectory();
        }

        private long GetLongValue(string name, long defaultValue)
        {
            var rawValue = _gitExecutor.GetLong("config --int " + SectionName + '.' + name);

            if (!rawValue.HasValue)
                return defaultValue;

            if (rawValue < 0)
                throw new ಠ_ಠ(name + " cannot be negative");

            return rawValue.Value;
        }

        private string GetStringValue(string name)
        {
            var rawValue = _gitExecutor.GetString("config " + SectionName + '.' + name);

            if (string.IsNullOrEmpty(rawValue))
                throw new ಠ_ಠ(name + " must be set");

            return rawValue;
        }

        private string GetCacheDirectory()
        {
            var rawValue = _gitExecutor.GetString("rev-parse --git-dir");

            if (string.IsNullOrEmpty(rawValue))
                throw new ಠ_ಠ("Error determining .git directory");

            return Path.Combine(rawValue, DirectoryName);
        }
    }
}