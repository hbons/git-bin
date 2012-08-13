using System;
using System.IO;
using System.Linq;
using GitBin.Remotes;

namespace GitBin.Commands
{
    public class PushCommand : ICommand
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRemote _remote;

        public PushCommand(
            ICacheManager cacheManager,
            IRemote remote,
            string[] args)
        {
            if (args.Length > 0)
                throw new ArgumentException();

            _cacheManager = cacheManager;
            _remote = remote;
        }

        public void Execute()
        {
            var filesInRemote = _remote.ListFiles();
            var filesInCache = _cacheManager.ListFiles();

            var filesToUpload = filesInCache.Except(filesInRemote).ToList();

            if (filesToUpload.Count == 0)
            {
                GitBinConsole.WriteLine("All chunks already present on remote");
            }
            else
            {
                if (filesToUpload.Count == 1)
                {
                    GitBinConsole.WriteLine("Uploading 1 chunk");
                }
                else
                {
                    GitBinConsole.WriteLine("Uploading {0} chunks", filesToUpload.Count);
                }

                for (int i = 0; i < filesToUpload.Count; i++)
                {
                    using (new RemoteProgressPrinter(i + 1, filesToUpload.Count, _remote))
                    {
                        var file = filesToUpload[i];
                        _remote.UploadFile(_cacheManager.GetPathForFile(file.Name), file.Name);
                    }
                }
            }
        }
    }
}