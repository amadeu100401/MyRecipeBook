#region USING
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.DeleteRecipe;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
#endregion

namespace UseCases.Recipe.UpdateRecipe;

public class DeleteRecipeUseCaseTests : CommonRepositoryBuild
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build();

        var useCase = CreateUseCase(user, recipe);

        Func<Task> act = async () => { await useCase.Execute(recipe.RecipeIdentifier); };

        _ = await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_No_Recipe()
    {
        (var user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => { await useCase.Execute(recipe.RecipeIdentifier); };

        _ = await act.Should().ThrowAsync<ErroOnValidationException>()
            .Where(ex => ex.ErrorsMesages.Contains(ResourceErroMensage.No_Recipe));
    }

    private DeleteRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null, MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var readRepository = GetBuildReadRepository(user.Id, recipe);
        var deleteRepository = GetBuildDeleteRepository(user.Id, recipe);
        var unityOfWork = UnityOfWorkBuilder.Build();

        return new DeleteRecipeUseCase(loggedUser, readRepository.Build(), deleteRepository.Build(), unityOfWork);
    }

    private RecipeDeleteOnlyRepositoryBuilder GetBuildDeleteRepository(long userId,
    MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var deleteRepositoryBuilder = new RecipeDeleteOnlyRepositoryBuilder();

        if (recipe != null)
            deleteRepositoryBuilder.Delete(userId, recipe.RecipeIdentifier);

        return deleteRepositoryBuilder;
    }
}
