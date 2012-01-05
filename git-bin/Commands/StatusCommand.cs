using System;
using System.Collections.Generic;
using System.Linq;
using GitBin.Remotes;

namespace GitBin.Commands
{
    public class StatusCommand : ICommand
    {
        public const string ShowRemoteArgument = "-r";

        private readonly ICacheManager _cacheManager;
        private readonly IRemote _remote;
        private readonly bool _shouldShowRemote;
        private readonly GitBinFileInfo[] _filesInLocalCache;

        public StatusCommand(
            ICacheManager cacheManager,
            IRemote remote,
            string[] args)
        {
            if (args.Length > 1)
                throw new ArgumentException();

            if (args.Length == 1)
            {
                if (args[0] == ShowRemoteArgument)
                {
                    _shouldShowRemote = true;
                }
                else
                {
                    throw new ArgumentException("status command only has one valid option: " + ShowRemoteArgument);
                }
            }

            _cacheManager = cacheManager;
            _remote = remote;

            _filesInLocalCache = _cacheManager.ListFiles();
        }

        public void Execute()
        {
            PrintStatusAboutCache();

            if (_shouldShowRemote)
            {
                PrintStatusAboutRemote();
            }
        }

        private void PrintStatusAboutCache()
        {
            GitBinConsole.WriteLineNoPrefix("Local cache:");
            GitBinConsole.WriteLineNoPrefix("  items: {0}", _filesInLocalCache.Length);
            GitBinConsole.WriteLineNoPrefix("  size:  {0}", GitBinFileInfoUtils.GetHumanReadableSize(_filesInLocalCache));
        }

        private void PrintStatusAboutRemote()
        {
            var remoteFiles = _remote.ListFiles();
            
            GitBinConsole.WriteLineNoPrefix("\nRemote repo:");
            GitBinConsole.WriteLineNoPrefix("  items: {0}", remoteFiles.Length);
            GitBinConsole.WriteLineNoPrefix("  size:  {0}", GitBinFileInfoUtils.GetHumanReadableSize(remoteFiles));

            var filesToPush = _filesInLocalCache.Except(remoteFiles).ToList();

            GitBinConsole.WriteLineNoPrefix("\nTo push:");
            GitBinConsole.WriteLineNoPrefix("  items: {0}", filesToPush.Count);
            GitBinConsole.WriteLineNoPrefix("  size:  {0}", GitBinFileInfoUtils.GetHumanReadableSize(filesToPush));
        }
    }
}