using Moq;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace CommonTestUtilities.Repositories;

public class RecipeUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeUpdateOnlyRepository> _repositopry;

    public RecipeUpdateOnlyRepositoryBuilder() => _repositopry = new Mock<IRecipeUpdateOnlyRepository>(); 

    public RecipeUpdateOnlyRepositoryBuilder Update(MyRecipeBook.Domain.Entities.Recipe recipe)
    {
        _repositopry.Setup(repository => repository.Update(recipe));
        return this;
    }

    public IRecipeUpdateOnlyRepository Build() => _repositopry.Object;
}
