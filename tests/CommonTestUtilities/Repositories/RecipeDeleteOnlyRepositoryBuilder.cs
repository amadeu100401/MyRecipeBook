#region USING
using Moq;
using MyRecipeBook.Domain.Repositories.Recipe;
#endregion

namespace CommonTestUtilities.Repositories;

public class RecipeDeleteOnlyRepositoryBuilder
{
    private Mock<IRecipeDeleteOnlyRepository> _repository;

    public RecipeDeleteOnlyRepositoryBuilder() => _repository = new Mock<IRecipeDeleteOnlyRepository>();

    public RecipeDeleteOnlyRepositoryBuilder Delete(long userIdentifier, Guid recipeIdentifier)
    {
        _repository.Setup(repository => repository.Delete(userIdentifier, recipeIdentifier));
        return this;
    }

    public IRecipeDeleteOnlyRepository Build() => _repository.Object;
}
