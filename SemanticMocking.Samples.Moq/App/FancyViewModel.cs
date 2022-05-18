using System.Windows.Input;

namespace SemanticMocking.SampleTests.App;

public class FancyViewModel
{
    private readonly IDialogService _dialogService;

    public FancyViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;

        DoSomeStuffCommand = new ActionCommand(DoSomeStuff);
    }

    public ICommand DoSomeStuffCommand { get; }
    
    private void DoSomeStuff()
    {
        _dialogService.ShowAlertAsync("Error", "This is a fialure!");
    }
}