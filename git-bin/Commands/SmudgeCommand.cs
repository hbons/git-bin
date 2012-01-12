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
            var document = GitBinDocument.FromYaml(new StreamReader(stdin));

            GitBinConsole.Write("Smudging {0}...", document.Filename);

            DownloadMissingFiles(document.ChunkHashes);

            OutputReassembledChunks(document.ChunkHashes);
        }

        private void DownloadMissingFiles(IEnumerable<string> chunkHashes)
        {
            var filesToDownload = _cacheManager.GetFilenamesNotInCache(chunkHashes);

            if (filesToDownload.Length > 0)
            {
                GitBinConsole.WriteLineNoPrefix(" Downloading {0} chunks...", filesToDownload.Length);

                for (int i = 0; i < filesToDownload.Length; i++)
                {
                    using (new RemoteProgressPrinter(i, filesToDownload.Length, _remote))
                    {
                        var file = filesToDownload[i];
                        _remote.DownloadFile(_cacheManager.GetPathForFile(file), file);
                    }
                }
            }
            else
            {
                GitBinConsole.WriteNoPrefix(Environment.NewLine);                
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