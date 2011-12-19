using System;
using GitBin.Commands;
using Objector;

namespace GitBin
{
    class Program
    {
        static int Main(string[] args)
        {
            var builder = new Builder();
            ApplicationRegistrations.Register(builder);
            var container = builder.Create();

            try
            {
                var commandFactory = container.Resolve<ICommandFactory>();
                ICommand command;

                try
                {
                    command = commandFactory.GetCommand(args);
                }
                catch (ArgumentException)
                {
                    commandFactory.GetShowUsageCommand().Execute();
                    return 1;
                }

                command.Execute();
            }
            catch (ಠ_ಠ lod)
            {
                GitBinConsole.WriteLine(lod.Message);
                return 2;
            }
            catch (Exception e)
            {
                GitBinConsole.WriteLine("Uncaught exception, please report this bug! " + e);
                return 3;
            }

            return 0;
        }
    }
}
