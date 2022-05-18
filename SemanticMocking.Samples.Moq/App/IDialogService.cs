using System.Threading.Tasks;

namespace SemanticMocking.SampleTests.App;

public interface IDialogService
{
    Task ShowMessageAsync(string title, string message);
}