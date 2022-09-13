namespace SemanticMocking.Samples.TaskApp;

public interface ICookingRecipeProvider
{
    /// <summary>
    /// Gets all the recipes that are stored in the persistence layer.
    /// </summary>
    /// <returns>An empty array if no recipes where persisted, yet.</returns>
    CookingRecipe[] GetAllRecipes();
    
    /// <summary>
    /// Saves a new recipe in the persistence layer.
    /// </summary>
    /// <param name="recipe">The recipe to persist.</param>
    /// <exception cref="System.Data.DataException">When persisting fails.</exception>
    void Create(CookingRecipe recipe);
}