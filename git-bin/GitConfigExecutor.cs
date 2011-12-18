using System;
using System.Diagnostics;

namespace GitBin
{
    public interface IGitConfigExecutor
    {
        long? GetLong(string name);
        string GetString(string name);
    }

    public class GitConfigExecutor : IGitConfigExecutor
    {
        private const int Success = 0;
        private const int MissingSectionOrKey = 1;
        private const int InvalidConfigFile = 3;

        public long? GetLong(string name)
        {
            var rawValue = ExecuteGit("--int", name);
            
            if (string.IsNullOrEmpty(rawValue))
                return null;

            return Convert.ToInt64(rawValue);
        }

        public string GetString(string name)
        {
            return ExecuteGit(string.Empty, name);
        }

        private static string ExecuteGit(string additionalConfigParameters, string name)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "git";
            process.StartInfo.Arguments = "config " + additionalConfigParameters + " git-bin." + name;
            process.Start();
            
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            switch (process.ExitCode)
            {
                case Success:
                case MissingSectionOrKey:
                    return output.Trim();

                case InvalidConfigFile:
                    throw new ಠ_ಠ(".gitconfig file is invalid");

                default:
                    throw new ಠ_ಠ("git config exited with error code [" + process.ExitCode + "] while retrieving config value for " + name);
            }
        }
    }
}