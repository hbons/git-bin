using System;

namespace GitBin.Commands
{
    public class ClearCommand : ICommand
    {
        public const string DryRunArgument = "-n";
        public const string ForceArgument = "-f";

        private readonly ICacheManager _cacheManager;
        private readonly bool _isDryRun;

        public ClearCommand(
            ICacheManager cacheManager,
            string[] args)
        {
            if (!TryParseArguments(args, out _isDryRun))
                throw new ArgumentException(string.Format("clear command requires either {0} or {1}", DryRunArgument, ForceArgument));

            _cacheManager = cacheManager;
        }

        public void Execute()
        {
            if (_isDryRun)
            {
                GitBinConsole.WriteLine("clear dry run: would remove " +
                    GitBinFileInfoUtils.GetHumanReadableSize(_cacheManager.ListFiles()));
            }
            else
            {
                _cacheManager.ClearCache();
            }
        }

        private bool TryParseArguments(string[] args, out bool isDryRun)
        {
            isDryRun = true;

            if (args.Length != 1)
                return false;

            switch (args[0])
            {
                case DryRunArgument:
                    return true;

                case ForceArgument:
                    isDryRun = false;
                    return true;

                default:
                    return false;
            }
        }
    }
}