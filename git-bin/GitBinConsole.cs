using System;

namespace GitBin
{
    public static class GitBinConsole
    {
        public static void Write(string message)
        {
            Console.Error.Write(message);
        }

        public static void WriteLine(string message, params object[] args)
        {
            Console.Error.WriteLine("[git-bin] " + message, args);
        }
    }
}