using Bogus;
using CommonTestUtilities.Extensions;
using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class RecipeBuilder
{
    public static Recipe Build(string? titleRequest = null, long? userId = null)
    {
        string recipeTilte = !string.IsNullOrWhiteSpace(titleRequest) ? titleRequest : new Faker().Random.Word();
        userId = userId ?? new Faker().Random.Long();

        return new Faker<Recipe>()
            .RuleFor(recipe => recipe.Id, f => f.Random.Long())
            .RuleFor(recipe => recipe.Title, recipeTilte )
            .RuleFor(recipe => recipe.Category, MyRecipeBook.Domain.Enum.Category.Breakfast.GetRandomEnumValue())
            .RuleFor(recipe => recipe.MethodPrepar, f => f.Lorem.Paragraph())
            .RuleFor(recipe => recipe.Ingredients, (f, recipe) => new List<Ingredients> { IngredinetBuilder.Build(recipe.Id) })
            .RuleFor(recipe => recipe.RecipeIdentifier, Guid.Empty)
            .RuleFor(recipe => recipe.UserId, userId);
    }
}
