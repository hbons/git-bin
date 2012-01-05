using System;
using GitBin.Commands;
using System.Linq;

namespace GitBin
{
    public interface ICommandFactory
    {
        ICommand GetCommand(string[] args);
        ICommand GetShowUsageCommand();
    }

    public class CommandFactory : ICommandFactory
    {
        public const string VersionArgument = "--version";
        public const string CleanArgument = "clean";
        public const string ClearArgument = "clear";
        public const string SmudgeArgument = "smudge";
        public const string PushArgument = "push";
        public const string StatusArgument = "status";

        private readonly Func<ShowUsageCommand> _showUsageCommandFactory;
        private readonly Func<VersionCommand> _versionCommandFactory;
        private readonly Func<string, PrintCommand> _printCommandFactory;
        private readonly Func<string[], CleanCommand> _cleanCommandFactory;
        private readonly Func<string[], SmudgeCommand> _smudgeCommandFactory;
        private readonly Func<string[], PushCommand> _pushCommandFactory;
        private readonly Func<string[], StatusCommand> _statusCommandFactory;
        private readonly Func<string[], ClearCommand> _clearCommandFactory;

        public CommandFactory(
            Func<ShowUsageCommand> showUsageCommandFactory,
            Func<VersionCommand> versionCommandFactory,
            Func<string, PrintCommand> printCommandFactory,
            Func<string[], CleanCommand> cleanCommandFactory,
            Func<string[], SmudgeCommand> smudgeCommandFactory,
            Func<string[], PushCommand> pushCommandFactory,
            Func<string[], StatusCommand> statusCommandFactory,
            Func<string[], ClearCommand> clearCommandFactory)
        {
            _showUsageCommandFactory = showUsageCommandFactory;
            _versionCommandFactory = versionCommandFactory;
            _printCommandFactory = printCommandFactory;
            _cleanCommandFactory = cleanCommandFactory;
            _smudgeCommandFactory = smudgeCommandFactory;
            _pushCommandFactory = pushCommandFactory;
            _statusCommandFactory = statusCommandFactory;
            _clearCommandFactory = clearCommandFactory;
        }

        public ICommand GetCommand(string[] args)
        {
            if (args.Length == 0)
                return GetShowUsageCommand();

            var commandArgument = args[0];
            var argsTail = args.Skip(1).ToArray();

            try
            {
                switch (commandArgument)
                {
                    case VersionArgument:
                        return _versionCommandFactory();

                    case CleanArgument:
                        return _cleanCommandFactory(argsTail);

                    case ClearArgument:
                        return _clearCommandFactory(argsTail);

                    case SmudgeArgument:
                        return _smudgeCommandFactory(argsTail);

                    case PushArgument:
                        return _pushCommandFactory(argsTail);

                    case StatusArgument:
                        return _statusCommandFactory(argsTail);
                }
            }
            catch (ArgumentException e)
            {
                if (!string.IsNullOrEmpty(e.Message))
                    return _printCommandFactory(e.Message);

                return GetShowUsageCommand();
            }

            return GetShowUsageCommand();
        }

        public ICommand GetShowUsageCommand()
        {
            return _showUsageCommandFactory();
        }
    }
}