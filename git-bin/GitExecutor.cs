using System;
using System.Diagnostics;

namespace GitBin
{
    public interface IGitExecutor
    {
        long? GetLong(string name);
        string GetString(string name);
    }

    public class GitExecutor : IGitExecutor
    {
        private const int Success = 0;
        private const int MissingSectionOrKey = 1;

        public long? GetLong(string arguments)
        {
            var rawValue = ExecuteGit(arguments);
            
            if (string.IsNullOrEmpty(rawValue))
                return null;

            return Convert.ToInt64(rawValue);
        }

        public string GetString(string arguments)
        {
            return ExecuteGit(arguments);
        }

        private static string ExecuteGit(string arguments)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "git";
            process.StartInfo.Arguments = arguments;
            process.Start();
            
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            switch (process.ExitCode)
            {
                case Success:
                case MissingSectionOrKey:
                    return output.Trim();

                default:
                    throw new ಠ_ಠ("git exited with error code [" + process.ExitCode + "] while executing command [" + arguments + ']');
            }
        }
    }
}