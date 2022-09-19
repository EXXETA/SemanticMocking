using NSubstitute;
using SemanticMocking.Abstractions;
using SemanticMocking.NSubstitute;

namespace SemanticMocking.CookingApp.Tests.NSubstitute.Mocks;

public class CookingRecipeProviderMock : NSubstituteMock<
    ICookingRecipeProvider, 
    CookingRecipeProviderMock.Arrangements, 
    CookingRecipeProviderMock.Assertions, 
    CookingRecipeProviderMock.Raises>
{
    public class Arrangements : BehaviourFor<CookingRecipeProviderMock>
    {
        public Arrangements HasRecipes(params CookingRecipe[] records)
        {
            Parent.Mock
                .GetAllRecipes()
                .Returns(records);
            return this;
        }

        public Arrangements HasNoRecipesYet()
        {
            return HasRecipes(Array.Empty<CookingRecipe>());
        }

        public Arrangements LoadingDataFailsWith(Exception exception)
        {
            Parent.Mock
                .GetAllRecipes()
                .Returns(_ => throw exception);
            return this;
        }

        /// <summary>
        /// Creating of new recipes will not fail.
        /// Note: Every created recipe will be returned in upcoming calls to <see cref="ICookingRecipeProvider.GetAllRecipes"/>
        /// as long as <see cref="HasRecipes"/> will not be arranged again.
        /// </summary>
        public Arrangements CreatingNewRecipesSucceeds()
        {
            // For the saving to succeed we don't need to setup anything.
            // But for following calls to GetAllRecipes() we can change the current mock setup.
            CookingRecipe[] oldRecipes = Parent.Mock.GetAllRecipes();

            Parent.Mock
                .When(mock => mock.Create(Arg.Any<CookingRecipe>()))
                .Do(call =>
                {
                    var recipe = call.Arg<CookingRecipe>();
                    var newRecipes = new List<CookingRecipe>(oldRecipes) {recipe};
                    HasRecipes(newRecipes.ToArray());
                });
            return this;
        }
    }
   
    public class Assertions : BehaviourFor<CookingRecipeProviderMock>
    {
    }

    public class Raises : BehaviourFor<CookingRecipeProviderMock>
    {
    }
}