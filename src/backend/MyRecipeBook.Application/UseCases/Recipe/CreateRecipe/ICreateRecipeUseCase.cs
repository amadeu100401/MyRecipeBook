using MyRecipeBook.Communication.Requests.RecipeRequest;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.CreateRecipe;

public interface ICreateRecipeUseCase
{
    public Task<ResponseRecipeJson> Execute(RequestRegisterRecipe request);
}
