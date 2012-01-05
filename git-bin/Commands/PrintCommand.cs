namespace GitBin.Commands
{
    public class PrintCommand : ICommand
    {
        private readonly string _message;

        public PrintCommand(string message)
        {
            _message = message;
        }

        public void Execute()
        {
            GitBinConsole.WriteLine(_message);
        }
    }
}