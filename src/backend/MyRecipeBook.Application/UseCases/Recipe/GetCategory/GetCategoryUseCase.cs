using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Enum;

namespace MyRecipeBook.Application.UseCases.Recipe.GetCategory;

public class GetCategoryUseCase : IGetCategoryUseCase
{
    public async Task<ResponseCategorysJson> Execute()
    {
        var category = GetCategory();
        return BuildResponse(category);
    }

    private static IDictionary<string, string> GetCategory()
    {
        var categoryValues = Enum.GetValues(typeof(Category)).Cast<Category>();
        var categorys = categoryValues.Cast<Category>().Select(x => x.ToString()).ToList();

        return CreateCategorysDictionary(categorys);
    }

    private static IDictionary<string, string> CreateCategorysDictionary(List<string> categorys)
    {
        var categorysDictionary = new Dictionary<string, string>();

        foreach (var item in categorys) 
        {
            var itemIndex = (categorys.IndexOf(item) + 1).ToString();

            categorysDictionary.Add(itemIndex, item);
        }

        return categorysDictionary; 
    }

    private ResponseCategorysJson BuildResponse(IDictionary<string, string> categorysDictionary)
    {
        return new ResponseCategorysJson 
        { 
            Categorys = categorysDictionary
        };

    }
}
