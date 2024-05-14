using CommonTestUtilities.Repositories;

namespace UseCases.Recipe;

public abstract class CommonRepositoryBuild
{
    public RecipeReadOnlyRepositoryBuilder GetBuildReadRepository(long userId,
    MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var readRepositoryBuilder = new RecipeReadOnlyRepositoryBuilder();

        if (recipe != null)
            readRepositoryBuilder.ExistsRecipe(userId, recipe); ;

        return readRepositoryBuilder;
    }
}
