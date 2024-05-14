using Bogus;
using CommonTestUtilities.Extensions;
using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class IngredinetBuilder
{
    public static Ingredients Build(long recipeId)
    {
        return new Faker<Ingredients>()
            .RuleFor(ingredient => ingredient.Id, recipeId)
            .RuleFor(ingrdient => ingrdient.Name, f => f.Commerce.ProductName())
            .RuleFor(ingrdient => ingrdient.Quantity, f => f.Random.Double())
            .RuleFor(ingrdient => ingrdient.UnitMeasure, MyRecipeBook.Domain.Enum.UnitMeasure.Gramas.GetRandomEnumValue())
            .RuleFor(ingrdient => ingrdient.IngredientsIdentifier, Guid.NewGuid());
    }
}
