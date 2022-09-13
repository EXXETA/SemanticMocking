namespace SemanticMocking.Samples.TaskApp;

public interface IDialogService
{
    System.Threading.Tasks.Task ShowMessageAsync(string title, string message);
}