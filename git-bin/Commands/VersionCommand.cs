using System;
using System.Reflection;

namespace GitBin.Commands
{
    public class VersionCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
    }
}