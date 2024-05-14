using Bogus;
using CommonTestUtilities.Extensions;
using MyRecipeBook.Communication.Enum;
using MyRecipeBook.Communication.Requests.RecipeRequest;

namespace CommonTestUtilities.Requests;

public class RequestRegisterRecipeJsonBuilder
{
    public RequestRegisterRecipe Build()
    {
        return new Faker<RequestRegisterRecipe>()
            .RuleFor(recipe => recipe.Title, (f) => f.Commerce.ProductName())
            .RuleFor(recipe => recipe.Category, Category.Breakfast.GetRandomEnumValue())
            .RuleFor(recipe => recipe.Ingredients, GenerateIngredientList())
            .RuleFor(recipe => recipe.TimePreparationMinutes, (f) => f.Random.Number(1, 999))
            .RuleFor(recipe => recipe.MethodPrepar, (f) => f.Lorem.Paragraph());
    }

    private static List<RequestRegisterIngredient> GenerateIngredientList()
    {
        var ingredientList = new List<RequestRegisterIngredient>();

        ingredientList.Add(new RequestRegisterIngredientJsonBuilder().Build());

        return ingredientList;
    }

    //private static T GetRandomEnumValue<T>() where T : Enum
    //{
    //    var values = Enum.GetValues(typeof(T));
    //    var random = new Random();
    //    return (T)values.GetValue(random.Next(values.Length));
    //}
}
