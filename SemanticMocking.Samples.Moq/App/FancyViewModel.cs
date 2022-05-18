using System;
using System.Linq;
using System.Windows.Input;

namespace SemanticMocking.SampleTests.App;

public class FancyViewModel
{
    private readonly IDataProvider _dataProvider;
    private readonly IDialogService _dialogService;

    public FancyViewModel(
        IDataProvider dataProvider,
        IDialogService dialogService)
    {
        _dataProvider = dataProvider;
        _dialogService = dialogService;

        DoSomeStuffCommand = new ActionCommand(DoSomeStuff);
    }

    public ICommand DoSomeStuffCommand { get; }
    
    private void DoSomeStuff()
    {
        try
        {
            var records = _dataProvider.GetRecords();

            if (!records.Any())
            {
                _ = _dialogService.ShowMessageAsync("Info", "No records found!");
                return;
            }
            
            // Fancy stuff comes here...
        }
        catch (Exception ex)
        {
            _ = _dialogService.ShowMessageAsync("Error", ex.Message);
        }
    }
}