namespace MyRecipeBook.Domain.Repositories.Recipe;

public interface IRecipeDeleteOnlyRepository
{
    public Task Delete(long userId, Guid recipeIdentifier);
}
