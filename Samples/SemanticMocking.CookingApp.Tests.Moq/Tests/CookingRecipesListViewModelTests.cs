using System.Data;
using System.Threading.Tasks;
using NUnit.Framework;
using SemanticMocking.CookingApp.Tests.Moq.Mocks;

namespace SemanticMocking.CookingApp.Tests.Moq.Tests;

public class CookingRecipesListViewModelTests
{
    private CookingRecipesListViewModel _sut = null!;

    private CookingRecipeProviderMock _cookingRecipeProvider = null!;
    private DialogServiceMock _dialogService = null!;
    private NavigationServiceMock _navigationService = null!;
    
    [SetUp]
    public void Setup()
    {
        _cookingRecipeProvider = new CookingRecipeProviderMock();
        _dialogService = new DialogServiceMock();
        _navigationService = new NavigationServiceMock();
        
        _sut = new CookingRecipesListViewModel(
            _cookingRecipeProvider.Mock.Object,
            _dialogService.Mock.Object,
            _navigationService.Mock.Object);
    }

    [Test]
    public async Task StartAsync_NoRecipesAvailable_ShowsEmptyMessage()
    {
        _cookingRecipeProvider.Arrange.HasNoRecipesYet();

        await _sut.StartAsync();

        Assert.AreEqual("No recipes, yet. Let's get started!", _sut.EmptyMessage);
    }
    
    [Test]
    public async Task StartAsync_LoadingFails_ShowsUserFriendlyErrorMessage()
    {
        _cookingRecipeProvider.Arrange.LoadingDataFailsWith(new DataException("unknown"));

        await _sut.StartAsync();

        _dialogService.Assert.DidShowError("Data could not be loaded.");
    }

    [Test]
    public async Task StartAsync_LoadingFails_NavigatesBack()
    {
        _cookingRecipeProvider.Arrange.LoadingDataFailsWith(new DataException("unknown"));

        await _sut.StartAsync();

        _navigationService.Assert.DidNavigateBack();
    }
    
    [Test]
    public async Task StartAsync_LoadingSucceeds_ShowsStoredRecipes()
    {
        _cookingRecipeProvider.Arrange.HasRecipes(new[]
        {
            new CookingRecipe {Id = "1", Title = "Pizza"},
            new CookingRecipe {Id = "2", Title = "Steak"}
        });

        await _sut.StartAsync();

        Assert.AreEqual(2, _sut.Recipes.Count);
    }

    [Test]
    public void CreateNewRecipeCommand_MissingTitle_ShowsRequiredFieldMessage()
    {
        _sut.NewTitle = string.Empty;
        
        _sut.CreateNewRecipeCommand.Execute(null);

        _dialogService.Assert.DidShowInfo("Title is required!");
    }
    
    [Test]
    public async Task CreateNewRecipeCommand_Succeeds_ShowsNewRecipe()
    {
        _cookingRecipeProvider.Arrange
            .HasRecipes(new CookingRecipe {Id = "1", Title = "Pizza"}, new CookingRecipe {Id = "2", Title = "Steak"})
            .CreatingNewRecipesSucceeds();
        
        await _sut.StartAsync();
        _sut.NewTitle = "Potatoes";
        
        _sut.CreateNewRecipeCommand.Execute(null);
        
        Assert.AreEqual(3, _sut.Recipes.Count);
    }
}