using System.Windows.Input;
using System.Windows;

namespace UDPChat.Models;

internal class BaseCommand : ICommand
{
    Action<object> _Execute;
    Predicate<object> _CanExecute;

    public BaseCommand(Action<object> executeCommand, Predicate<object> canExecute)
    {
        _Execute = executeCommand;
        _CanExecute = canExecute;
    }

    public BaseCommand(Action<object> executeCommand)
    {
        _Execute = executeCommand;
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
        try
        {
            _Execute(parameter);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
}
