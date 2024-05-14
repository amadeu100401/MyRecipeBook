namespace MyRecipeBook.Domain.Repositories.Recipe;

public interface IRecipeWriteOnlyRepository
{
    public Task AddRecipe(MyRecipeBook.Domain.Entities.Recipe recipe);
}
