using System;
using System.Windows.Input;

namespace SemanticMocking.SampleTests.App;

public class ActionCommand : ICommand
{
    private readonly Action _executeAction;

    public ActionCommand(Action executeAction)
    {
        _executeAction = executeAction;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _executeAction.Invoke();
    }

    public event EventHandler? CanExecuteChanged;
}