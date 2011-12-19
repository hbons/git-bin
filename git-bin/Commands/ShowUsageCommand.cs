using System;

namespace GitBin.Commands
{
    public class ShowUsageCommand : ICommand
    {
        public void Execute()
        {
            Console.Error.WriteLine("usage: git bin [--version]");
            Console.Error.WriteLine("               <command> [<args>]\n");
            Console.Error.WriteLine("List of available commands:");
            Console.Error.WriteLine("  clean    Clean filter. Should only be used with .gitattributes filtering.");
            Console.Error.WriteLine("  gc       Clean up unnecessary files in the local cache directory.");
            Console.Error.WriteLine("  init     Install helper hooks.");
            Console.Error.WriteLine("  push     Upload changed files to the remote file repository.");
            Console.Error.WriteLine("  smudge   Smudge filter. Should only be used with .gitattributes filtering.");
            Console.Error.WriteLine("\nRun 'git bin help <command>' for more information on a specific command.");
        }
    }
}