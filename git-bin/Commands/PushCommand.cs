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
            var filesInRemote = _remote.ListFiles().Select(rfi => rfi.Name);
            var filesInCache = _cacheManager.ListFiles();

            var filesToUpload = filesInCache.Except(filesInRemote).ToList();

            GitBinConsole.WriteLine("Uploading {0} chunks", filesToUpload.Count);

            for (int i = 0; i < filesToUpload.Count; i++)
            {
                using (new RemoteProgressPrinter(i, filesToUpload.Count, _remote))
                {
                    var file = filesToUpload[i];
                    _remote.UploadFile(_cacheManager.GetPathForFile(file), file);
                }
            }
        }
    }
}