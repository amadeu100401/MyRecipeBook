namespace MyRecipeBook.Domain.Repositories.Recipe;

public interface IRecipeReadOnlyRepository
{
    public Task<List<MyRecipeBook.Domain.Entities.Recipe>> GetRecipe(long userId);
    public Task<bool> ExistsRecipe(long userId, Guid recipeIdentifier);
}
