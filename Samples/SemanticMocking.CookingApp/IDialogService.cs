namespace SemanticMocking.CookingApp;

public interface IDialogService
{
    System.Threading.Tasks.Task ShowMessageAsync(string title, string message);
}