using System;
using System.Security.Cryptography;
using System.Text;

namespace GitBin.Commands
{
    public class CleanCommand : ICommand
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly string _filename;

        public bool CanExecute { get; private set; }

        public CleanCommand(
            IConfigurationProvider configurationProvider,
            string[] args)
        {
            _configurationProvider = configurationProvider;

            if (args.Length != 1)
            {
                this.CanExecute = false;
            }
            else
            {
                this.CanExecute = true;
                _filename = args[0];
            }
        }

        public void Execute()
        {
            Console.Error.WriteLine("Chunk size is " + _configurationProvider.ChunkSize);


            var stdin = Console.OpenStandardInput();

            var hasher = new SHA256Managed();
            byte[] hash= hasher.ComputeHash(stdin);

            Console.Error.WriteLine("For file {0}, computed hash of {1}", _filename, ConvertHashToString(hash));
        }

        private static string ConvertHashToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }
    }
}