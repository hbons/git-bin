using System;

namespace GitBin.Commands
{
    public class ShowUsageCommand : ICommand
    {
        public void Execute()
        {
            GitBinConsole.WriteLineNoPrefix("usage: git bin [--version]");
            GitBinConsole.WriteLineNoPrefix("               <command> [<args>]");
            GitBinConsole.WriteNoPrefix(Environment.NewLine);
            GitBinConsole.WriteLineNoPrefix("List of available commands:");
            GitBinConsole.WriteLineNoPrefix("  clean    Clean filter. Should only be used with .gitattributes filtering");
//            GitBinConsole.WriteLineNoPrefix("  gc       Clean up unnecessary files in the local cache directory");
//            GitBinConsole.WriteLineNoPrefix("  init     Install helper hooks");
            GitBinConsole.WriteLineNoPrefix("  push     Upload changed files to the remote file repository");
            GitBinConsole.WriteLineNoPrefix("  smudge   Smudge filter. Should only be used with .gitattributes filtering");
            GitBinConsole.WriteLineNoPrefix("  status   Display status of the local cache. [-r], include the remote repo");
        }
    }
}