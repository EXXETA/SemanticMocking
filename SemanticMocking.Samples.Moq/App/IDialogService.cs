using System.Threading.Tasks;

namespace SemanticMocking.SampleTests.App;

public interface IDialogService
{
    Task ShowAlertAsync(string title, string message);
}