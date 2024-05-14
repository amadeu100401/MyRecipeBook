#region USING
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Application.UseCases.Dashboard;
using MyRecipeBook.Domain.Entities;
#endregion

namespace UseCases.Dashboard;

public class GetDashboardUseCaseTest
{
    [Fact]
    public async Task Success_Without_Recipe()
    {
        (var user, _ ) = UserBuilder.Build();

        var request = RequestDashboardJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.UsersRecipes.Should().HaveCount(0);   
    }

    [Fact]
    public async Task Success_With_Recipe()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestDashboardJsonBuilder.Build();

        var reciple = RecipeBuilder.Build(titleRequest: string.Empty, userId: user.Id);

        var useCase = CreateUseCase(user, reciple);

        var result = await useCase.Execute(request);

        result.UsersRecipes.Should().HaveCount(1)
            .And.Contain(recipe => reciple.UserId == user.Id);
    }

    [Fact]
    public async Task Success_With_Title_Recipe()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestDashboardJsonBuilder.Build();

        var reciple = RecipeBuilder.Build(titleRequest: string.Empty, userId: user.Id);
        request.TitleOrIngredientName = reciple.Ingredients[0].Name;

        var useCase = CreateUseCase(user, reciple);

        var result = await useCase.Execute(request);

        result.UsersRecipes.Should().HaveCount(1)
            .And.Contain(recipe => reciple.Ingredients
            .Any(ingredient => ingredient.Name.Equals(request.TitleOrIngredientName)));
    }

    [Fact]
    public async Task Success_With_MaxTime_Recipe()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestDashboardJsonBuilder.Build();

        var reciple = RecipeBuilder.Build(titleRequest: string.Empty, userId: user.Id);
        request.MaxTime = reciple.TimePreparationMinutes;

        var useCase = CreateUseCase(user, reciple);

        var result = await useCase.Execute(request);

        result.UsersRecipes.Should().HaveCount(1)
            .And.Contain(recipe => reciple.TimePreparationMinutes <= request.MaxTime);
    }

    [Fact]
    public async Task Success_With_Category_Recipe()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestDashboardJsonBuilder.Build();

        var reciple = RecipeBuilder.Build(titleRequest: string.Empty, userId: user.Id);
        request.Category = (MyRecipeBook.Communication.Enum.Category)reciple.Category;

        var useCase = CreateUseCase(user, reciple);

        var result = await useCase.Execute(request);

        result.UsersRecipes.Should().HaveCount(1)
            .And.Contain(recipe => reciple.Category.IsEqualTo((MyRecipeBook.Communication.Enum.Category)request.Category, reciple.Category));
    }


    private static GetDashboardUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var readRepository = new RecipeReadOnlyRepositoryBuilder();
        var loggedUser = LoggedUserBuilder.Build(user);
        var mapper = MapperBuilder.Build();

        if(recipe != null)
            readRepository.GetRecipe(user.Id, recipe);

        return new GetDashboardUseCase(readRepository.Build(), loggedUser, mapper);
    }
}
