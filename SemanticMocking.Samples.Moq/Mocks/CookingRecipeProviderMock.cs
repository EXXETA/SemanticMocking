using System;
using System.Collections.Generic;
using Moq;
using SemanticMocking.Abstractions;
using SemanticMocking.Moq;
using SemanticMocking.Samples.TaskApp;

namespace SemanticMocking.SampleTests.Mocks;

public class CookingRecipeProviderMock : MoqMock<
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
                .Setup(mock => mock.GetAllRecipes())
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
                .Setup(mock => mock.GetAllRecipes())
                .Throws(exception);
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
            CookingRecipe[] oldRecipes = Parent.Mock.Object.GetAllRecipes();

            Parent.Mock
                .Setup(mock => mock.Create(It.IsAny<CookingRecipe>()))
                .Callback<CookingRecipe>(recipe =>
                {
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