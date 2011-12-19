using System;

namespace GitBin
{
    public static class GitBinConsole
    {
        private const string Prefix = "[git-bin] ";

        public static void Write(string message)
        {
            Console.Error.Write(Prefix + message);
        }

        public static void Write(string message, params object[] args)
        {
            Console.Error.Write(Prefix + message, args);
        }

        public static void WriteLine(string message, params object[] args)
        {
            Console.Error.WriteLine(Prefix + message, args);
        }

        public static void WriteNoPrefix(string message)
        {
            Console.Error.Write(message);
        }

        public static void WriteNoPrefix(string message, params object[] args)
        {
            Console.Error.Write(message, args);
        }

        public static void WriteLineNoPrefix(string message, params object[] args)
        {
            Console.Error.WriteLine(message, args);
        }

        public static void Flush()
        {
            Console.Error.Flush();
        }
    }
}