using MyRecipeBook.Communication.Requests.RecipeRequest;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.UpdateRecipe;

public interface IUpdateRecipeUseCase
{
    public Task<ResponseRecipeJson> Execute(Guid recipeIdentifier, RequestRegisterRecipe request);
}
