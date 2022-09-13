using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SemanticMocking.Samples.TaskApp;

public class CookingRecipesListViewModel
{
    private readonly ICookingRecipeProvider _cookingRecipeProvider;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;

    public CookingRecipesListViewModel(
        ICookingRecipeProvider cookingRecipeProvider,
        IDialogService dialogService,
        INavigationService navigationService)
    {
        _cookingRecipeProvider = cookingRecipeProvider;
        _dialogService = dialogService;
        _navigationService = navigationService;

        Recipes = new ObservableCollection<CookingRecipe>();
        CreateNewRecipeCommand = new ActionCommand(CreateNewRecipe);
    }

    /// <summary>
    /// Gets or sets the title for a new recipe that should be stored in the database.
    /// </summary>
    public string? NewTitle { get; set; }
    
    /// <summary>
    /// Stores a new recipe with the title given in <see cref="NewTitle"/> in the database.
    /// </summary>
    public ICommand CreateNewRecipeCommand { get; }

    /// <summary>
    /// Gets a text that should be shown when no recipes have been added, yet.
    /// </summary>
    public string? EmptyMessage { get; private set; }

    public ObservableCollection<CookingRecipe> Recipes { get; private set; }

    /// <summary>
    /// This is a default method that will be called when navigating to this view model.
    /// It can do (long) async operations to initialize the view model. 
    /// </summary>
    public async Task StartAsync()
    {
        try
        {
            var allRecipes = _cookingRecipeProvider.GetAllRecipes();
            Recipes = new ObservableCollection<CookingRecipe>(allRecipes);

            if (!allRecipes.Any())
            {
                EmptyMessage = "No recipes, yet. Let's get started!";
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowMessageAsync("Error", "Data could not be loaded.");
            _navigationService.NavigateBack();
        }
    }
    
    private async void CreateNewRecipe()
    {
        if (string.IsNullOrEmpty(NewTitle))
        {
            await _dialogService.ShowMessageAsync("Info", "Title is required!");
            return;
        }
        
        _cookingRecipeProvider.Create(new CookingRecipe
        {
            Id = Guid.NewGuid().ToString(),
            Title = NewTitle
        });

        // We could just add the new recipe to the existing collection but this
        // way is is for the sake of the example test.
        Recipes = new ObservableCollection<CookingRecipe>(_cookingRecipeProvider.GetAllRecipes());
    }
}