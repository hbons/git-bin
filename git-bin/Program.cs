using System;
using Objector;

namespace GitBin
{
    class Program
    {
        static int Main(string[] args)
        {
//            Console.Error.WriteLine("[git-bin] Received command line: `{0}`", string.Join(" ", args));

            var builder = new Builder();
            ApplicationRegistrations.Register(builder);
            var container = builder.Create();

            try
            {
                var commandFactory = container.Resolve<ICommandFactory>();
                var command = commandFactory.GetCommand(args);

                if (command.CanExecute)
                {
                    command.Execute();
                }
                else
                {
                    commandFactory.GetShowUsageCommand().Execute();
                }
            }
            catch (ಠ_ಠ lod)
            {
                GitBinConsole.WriteLine(lod.Message);
                return 1;
            }
            catch (Exception e)
            {
                GitBinConsole.WriteLine("Uncaught exception, please report this bug! " + e);
                return 2;
            }

            return 0;
        }
    }
}
