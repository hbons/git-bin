using System;
using System.Security.Cryptography;
using System.Text;

namespace GitBin.Commands
{
    public class CleanCommand : ICommand
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICacheManager _cacheManager;
        private readonly string _filename;

        public bool CanExecute { get; private set; }

        public CleanCommand(
            IConfigurationProvider configurationProvider,
            ICacheManager cacheManager,
            string[] args)
        {
            _configurationProvider = configurationProvider;
            _cacheManager = cacheManager;

            if (args.Length != 1)
            {
                this.CanExecute = false;
            }
            else
            {
                this.CanExecute = true;
                _filename = args[0];
            }
        }

        public void Execute()
        {
            GitBinConsole.WriteLine("Cleaning {0}", _filename);

            var cleanedDocument = new GitBinDocument(_filename);

            var chunkBuffer = new byte[_configurationProvider.ChunkSize];
            int numberOfBytesRead;

            var stdin = Console.OpenStandardInput();

            do
            {
                numberOfBytesRead = stdin.Read(chunkBuffer, 0, chunkBuffer.Length);

                if (numberOfBytesRead > 0)
                {
                    var hash = GetHashForChunk(chunkBuffer, numberOfBytesRead);
                    _cacheManager.WriteFileToCache(hash, chunkBuffer, numberOfBytesRead);
                    cleanedDocument.RecordChunk(hash);
                }
            } while (numberOfBytesRead == chunkBuffer.Length);

            Console.Write(cleanedDocument.ToString());
//            GitBinConsole.Write(cleanedDocument.ToString());
            Console.Out.Flush();
        }

        private static string GetHashForChunk(byte[] chunkBuffer, int chunkLength)
        {
            var hasher = new SHA256Managed();

            byte[] hashBytes = hasher.ComputeHash(chunkBuffer, 0, chunkLength);
            var hashString = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

            return hashString;
        }
    }
}