using System;
using System.Collections.Generic;
using System.IO;
using GitBin.Remotes;

namespace GitBin.Commands
{
    public class SmudgeCommand : ICommand
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRemote _remote;

        public SmudgeCommand(
            ICacheManager cacheManager,
            IRemote remote,
            string[] args)
        {
            if (args.Length != 0)
                throw new ArgumentException();

            _cacheManager = cacheManager;
            _remote = remote;
        }

        public void Execute()
        {
            var stdin = Console.OpenStandardInput();
            var document = new GitBinDocument(stdin);

            GitBinConsole.WriteLine("Smudging {0}...", document.Filename);

            DownloadMissingFiles(document.ChunkHashes);

            OutputReassembledChunks(document.ChunkHashes);
        }

        private void DownloadMissingFiles(IEnumerable<string> chunkHashes)
        {
            var filesToDownload = _cacheManager.GetFilenamesNotInCache(chunkHashes);

            if (filesToDownload.Length > 0)
            {
                GitBinConsole.Write(" downloading {0} chunks", filesToDownload.Length);

                foreach (var file in filesToDownload)
                {
                    _remote.DownloadFileTo(_cacheManager.GetPathForFile(file), file);
//                    var stream = _remote.DownloadFile(file);
//                    _cacheManager.WriteFileToCache(file, stream);
                }
            }
        }

        private void OutputReassembledChunks(IEnumerable<string> chunkHashes)
        {
            var stdout = Console.OpenStandardOutput();

            foreach (var chunkHash in chunkHashes)
            {
                var chunkData = _cacheManager.ReadFileFromCache(chunkHash);
                stdout.Write(chunkData, 0, chunkData.Length);
            }

            stdout.Flush();
        }
    }
}