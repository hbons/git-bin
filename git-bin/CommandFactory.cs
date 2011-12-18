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
        public const string CleanArgument = "clean";
        public const string SmudgeArgument = "smudge";

        private readonly Func<ShowUsageCommand> _showUsageCommandFactory;
        private readonly Func<string[], CleanCommand> _cleanCommandFactory;
        private readonly Func<string[], SmudgeCommand> _smudgeCommandFactory;

        public CommandFactory(
            Func<ShowUsageCommand> showUsageCommandFactory,
            Func<string[], CleanCommand> cleanCommandFactory,
            Func<string[], SmudgeCommand> smudgeCommandFactory)
        {
            _showUsageCommandFactory = showUsageCommandFactory;
            _cleanCommandFactory = cleanCommandFactory;
            _smudgeCommandFactory = smudgeCommandFactory;
        }

        public ICommand GetCommand(string[] args)
        {
            if (args.Length == 0)
                return GetShowUsageCommand();

            var commandArgument = args[0];
            var argsTail = args.Skip(1).ToArray();

            if (commandArgument == CleanArgument)
                return _cleanCommandFactory(argsTail);

            if (commandArgument == SmudgeArgument)
                return _smudgeCommandFactory(argsTail);

            return GetShowUsageCommand();
        }

        public ICommand GetShowUsageCommand()
        {
            return _showUsageCommandFactory();
        }
    }
}