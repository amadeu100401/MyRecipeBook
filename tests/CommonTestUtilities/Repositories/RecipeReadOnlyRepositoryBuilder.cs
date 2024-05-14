using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories;

public class RecipeReadOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeReadOnlyRepository> _repository;

    public RecipeReadOnlyRepositoryBuilder() => _repository = new Mock<IRecipeReadOnlyRepository>();

    public RecipeReadOnlyRepositoryBuilder GetRecipe(long userId, Recipe recipe)
    {
        _repository.Setup(repository => repository.GetRecipe(userId)).ReturnsAsync(new List<Recipe> {recipe});
        return this; //Devolvendo a propia instancia
    }

    public RecipeReadOnlyRepositoryBuilder ExistsRecipe(long userId, Recipe recipe)
    {
        _repository.Setup(repository => repository.ExistsRecipe(userId, recipe.RecipeIdentifier)).ReturnsAsync(true);
        return this; //Devolvendo a propia instancia
    }

    public IRecipeReadOnlyRepository Build() => _repository.Object;


}
