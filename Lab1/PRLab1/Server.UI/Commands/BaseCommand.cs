using System.Windows.Input;

namespace Server.UI.Commands
{
    internal class BaseCommand : ICommand
    {
        Action<object> _Execute;
        Predicate<object> _CanExecute;

        public BaseCommand(Action<object> executeCommand, Predicate<object?> canExecute)
        {
            this._Execute = executeCommand;
            this._CanExecute = canExecute;
        }

        public BaseCommand(Action<object> executeCommand)
        {
            this._Execute = executeCommand;
        }

        public bool CanExecute(object parameter)
        {
            if (_CanExecute == null)
                return true;
            else
                return _CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _Execute(parameter);
        }
    }
}
