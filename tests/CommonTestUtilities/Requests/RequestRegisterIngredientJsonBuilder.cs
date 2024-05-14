using Bogus;
using MyRecipeBook.Communication.Enum;
using MyRecipeBook.Communication.Requests.RecipeRequest;
namespace CommonTestUtilities.Requests;

public class RequestRegisterIngredientJsonBuilder
{
   public RequestRegisterIngredient Build()
    {
        return new Faker<RequestRegisterIngredient>()
            .RuleFor(ingredient => ingredient.Name, (f) => f.Commerce.ProductName())
            .RuleFor(ingredient => ingredient.Quantity, (f) => f.Random.Number(1, 999))
            .RuleFor(ingredient => ingredient.UnitMeasure, GetRandomEnumValue<UnitMeasure>);
    }

    private static T GetRandomEnumValue<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        var random = new Random();
        return (T)values.GetValue(random.Next(values.Length));
    }
}
