#region USING
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.UpdateRecipe;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
#endregion

namespace UseCases.Recipe.UpdateRecipe;

public class UpdateRecipeUseCaseTests : CommonRepositoryBuild
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var request = new RequestRegisterRecipeJsonBuilder().Build();

        var recipe = RecipeBuilder.Build(titleRequest: request.Title);

        request.Title = "TesteQualquer";

        var useCase = CreateUsecase(user, recipe);

        var result = await useCase.Execute(recipe.RecipeIdentifier, request);

        result.Title.Should().NotBeNullOrWhiteSpace().And.Be(request.Title);    
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = new RequestRegisterRecipeJsonBuilder().Build();

        var recipe = RecipeBuilder.Build(titleRequest: request.Title);

        request.Title = string.Empty;

        var useCase = CreateUsecase(user, recipe);

        Func<Task> act = async () => { await useCase.Execute(recipe.RecipeIdentifier, request); } ;

        await act.Should().ThrowAsync<ErroOnValidationException>().Where(ex => ex.ErrorsMesages.Contains(ResourceErroMensage.Title_Empty));
    }

    [Fact]
    public async Task Error_Ingredients_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = new RequestRegisterRecipeJsonBuilder().Build();

        var recipe = RecipeBuilder.Build(titleRequest: request.Title);

        request.Ingredients = null;

        var useCase = CreateUsecase(user, recipe);

        Func<Task> act = async () => { await useCase.Execute(recipe.RecipeIdentifier, request); };

        await act.Should().ThrowAsync<ErroOnValidationException>().Where(ex => ex.ErrorsMesages.Contains(ResourceErroMensage.Ingredients_Empty));
    }

    private UpdateRecipeUseCase CreateUsecase(MyRecipeBook.Domain.Entities.User? user = null, MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var readrepository = GetBuildReadRepository(user.Id, recipe);
        var updateRepository = GetBuildUpdateRepository(recipe);
        var loggedUser = LoggedUserBuilder.Build(user);
        var mapper = MapperBuilder.Build();
        var unityOfWork = UnityOfWorkBuilder.Build();   

        return new UpdateRecipeUseCase( updateRepository.Build(), readrepository.Build(), loggedUser, mapper, unityOfWork);
    }

    public RecipeUpdateOnlyRepositoryBuilder GetBuildUpdateRepository(MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var updateRepositoryBuilder = new RecipeUpdateOnlyRepositoryBuilder();

        if (recipe != null)
            updateRepositoryBuilder.Update(recipe); ;

        return updateRepositoryBuilder;
    }
}
