#region using
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.CreateRecipe;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
#endregion

namespace UseCases.Recipe.CreateRecipe;

public class CreateRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _ ) = UserBuilder.Build();

        var request = new RequestRegisterRecipeJsonBuilder().Build();

        var useCase = await CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Title.Should().NotBeNull();
        result.Ingredients.Should().NotBeNull();
        result.MethodPrepar.Should().NotBeNull();
        result.RecipeIdentifier.Should().NotBeEmpty();
        result.TimePreparationMinutes.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Erro_Title_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = new RequestRegisterRecipeJsonBuilder().Build();
        request.Title = string.Empty;

        var useCase = await CreateUseCase(user);

       Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErroOnValidationException>()
            .Where(ex => ex.ErrorsMesages.Contains(ResourceErroMensage.Title_Empty));
    }

    [Fact]
    public async Task Erro_Ingredient_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = new RequestRegisterRecipeJsonBuilder().Build();
        request.Ingredients = null;

        var useCase = await CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErroOnValidationException>()
            .Where(ex => ex.ErrorsMesages.Contains(ResourceErroMensage.Ingredients_Empty));
    }

    [Fact]
    public async Task Erro_Ingredient_Title_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = new RequestRegisterRecipeJsonBuilder().Build();
        request.Ingredients.ForEach(ingredient => ingredient.Name = string.Empty);

        var useCase = await CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErroOnValidationException>()
            .Where(ex => ex.ErrorsMesages.Contains(ResourceErroMensage.Ingredient_Name_Empty));
    }

    private static async Task<CreateRecipeUseCase> CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var recipeWriteOnly = RecipeWriteOnlyRepositoryBuilder.Build();
        var userReadOnly = new UserReadOnlyRepositoryBuilder().Build();
        var mapper = MapperBuilder.Build();
        var unityOfWork = UnityOfWorkBuilder.Build();

        return new CreateRecipeUseCase(loggedUser, recipeWriteOnly, userReadOnly, mapper, unityOfWork);
    }
}
