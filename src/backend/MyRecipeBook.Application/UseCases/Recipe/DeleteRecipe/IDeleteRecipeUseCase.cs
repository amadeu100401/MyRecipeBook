namespace MyRecipeBook.Application.UseCases.Recipe.DeleteRecipe;

public interface IDeleteRecipeUseCase
{
    public Task Execute(Guid recipeIdentifier);
}
