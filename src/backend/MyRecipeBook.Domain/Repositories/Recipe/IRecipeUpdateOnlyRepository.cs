namespace MyRecipeBook.Domain.Repositories.Recipe;

public interface IRecipeUpdateOnlyRepository 
{
    public Task Update(MyRecipeBook.Domain.Entities.Recipe recipe);
}
