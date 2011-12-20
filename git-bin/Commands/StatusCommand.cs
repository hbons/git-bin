using System;
using System.Linq;
using GitBin.Remotes;

namespace GitBin.Commands
{
    public class StatusCommand : ICommand
    {
        public const string ShowRemoteArgument = "-r";
        private static string[] Suffixes = new[] {"B", "k", "M", "G", "T", "P", "E"};

        private readonly ICacheManager _cacheManager;
        private readonly IRemote _remote;
        private readonly bool _shouldShowRemote;

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
                    throw new ArgumentException();
                }
            }

            _cacheManager = cacheManager;
            _remote = remote;
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
            GitBinConsole.WriteLineNoPrefix("  items: {0}", _cacheManager.ListFiles().Length);
            GitBinConsole.WriteLineNoPrefix("  size:  {0}", GetHumanReadableSize(_cacheManager.GetSizeOnDisk()));
        }

        private void PrintStatusAboutRemote()
        {
            var remoteFiles = _remote.ListFiles();
            var sizeOfRemote = remoteFiles.Sum(rfi => rfi.Size);

            GitBinConsole.WriteLineNoPrefix("Remote repo:");
            GitBinConsole.WriteLineNoPrefix("  items: {0}", remoteFiles.Length);
            GitBinConsole.WriteLineNoPrefix("  size:  {0}", GetHumanReadableSize(sizeOfRemote));
        }

        public static string GetHumanReadableSize(long numberOfBytes)
        {
            int suffixIndex = 0;
            int increment = 1024;
            double scaledNumberOfBytes = numberOfBytes;

            if (numberOfBytes > 0)
            {
                while (scaledNumberOfBytes >= increment)
                {
                    suffixIndex++;
                    scaledNumberOfBytes /= increment;
                }

                if (Math.Abs(scaledNumberOfBytes - 0) < 0.1)
                {
                    scaledNumberOfBytes = 1;
                }
            }

            return string.Format("{0}{1}", scaledNumberOfBytes.ToString("0.#"), Suffixes[suffixIndex]);
        }
    }
}