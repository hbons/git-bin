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
        public const string SmudgeArgument = "smudge";
        public const string PushArgument = "push";

        private readonly Func<ShowUsageCommand> _showUsageCommandFactory;
        private readonly Func<VersionCommand> _versionCommandFactory;
        private readonly Func<string[], CleanCommand> _cleanCommandFactory;
        private readonly Func<string[], SmudgeCommand> _smudgeCommandFactory;
        private readonly Func<string[], PushCommand> _pushCommandFactory;

        public CommandFactory(
            Func<ShowUsageCommand> showUsageCommandFactory,
            Func<VersionCommand> versionCommandFactory,
            Func<string[], CleanCommand> cleanCommandFactory,
            Func<string[], SmudgeCommand> smudgeCommandFactory,
            Func<string[], PushCommand> pushCommandFactory)
        {
            _showUsageCommandFactory = showUsageCommandFactory;
            _versionCommandFactory = versionCommandFactory;
            _cleanCommandFactory = cleanCommandFactory;
            _smudgeCommandFactory = smudgeCommandFactory;
            _pushCommandFactory = pushCommandFactory;
        }

        public ICommand GetCommand(string[] args)
        {
            if (args.Length == 0)
                return GetShowUsageCommand();

            var commandArgument = args[0];
            var argsTail = args.Skip(1).ToArray();

            try
            {
                if (commandArgument == VersionArgument)
                    return _versionCommandFactory();

                if (commandArgument == CleanArgument)
                    return _cleanCommandFactory(argsTail);

                if (commandArgument == SmudgeArgument)
                    return _smudgeCommandFactory(argsTail);

                if (commandArgument == PushArgument)
                    return _pushCommandFactory(argsTail);
            }
            catch (ArgumentException)
            {
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