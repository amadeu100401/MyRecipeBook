using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.GetCategory;

public interface IGetCategoryUseCase
{
    public Task<ResponseCategorysJson> Execute();
}
