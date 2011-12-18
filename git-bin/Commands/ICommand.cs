namespace GitBin.Commands
{
    public interface ICommand
    {
        bool CanExecute { get;  }

        void Execute();
    }
}