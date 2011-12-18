using System.IO;

namespace GitBin
{
    public interface ICacheManager
    {
        void WriteFileToCache(string filename, byte[] contents, int contentLength);
    }

    public class CacheManager : ICacheManager
    {
        private readonly DirectoryInfo _cacheDirectoryInfo;

        public CacheManager(IConfigurationProvider configurationProvider)
        {
            _cacheDirectoryInfo = Directory.CreateDirectory(configurationProvider.CacheDirectory);
        }

        public void WriteFileToCache(string filename, byte[] contents, int contentLength)
        {
            var path = Path.Combine(_cacheDirectoryInfo.FullName, filename);

            if (!File.Exists(path))
            {
                var filestream = File.Create(path, contentLength, FileOptions.WriteThrough);
                filestream.Write(contents, 0, contentLength);
                filestream.Close();
            }
        } 
    }
}