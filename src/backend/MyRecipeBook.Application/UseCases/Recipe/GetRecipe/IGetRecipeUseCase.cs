using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.GetRecipe;

public interface IGetRecipeUseCase
{
    public Task<ResponseRecipeJson> Execute(Guid recipeIdentifier);
}
