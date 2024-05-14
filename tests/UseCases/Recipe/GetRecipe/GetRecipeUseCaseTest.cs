#region using
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Application.UseCases.Recipe.GetRecipe;
using MyRecipeBook.Domain.Entities;
#endregion

namespace UseCases.Recipe.GetRecipe;

public class GetRecipeUseCaseTest
{
    [Fact]
    public async Task Success_Without_Recipes()
    {
        (var user, _ ) = UserBuilder.Build();

        var request = RequestDashboardJsonBuilder.Build();

        var recipe = RecipeBuilder.Build(request.TitleOrIngredientName);

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(Guid.NewGuid());
         
        result.Should().BeNull();
    }

    [Fact]
    public async Task Success_With_Recipes_Recipe_Title() 
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestDashboardJsonBuilder.Build();

        var recipe = RecipeBuilder.Build(titleRequest: request.TitleOrIngredientName);

        var useCase = CreateUseCase(user, recipe);

        var result = await useCase.Execute(recipe.RecipeIdentifier);

        result.RecipeIdentifier.Should().Be(recipe.RecipeIdentifier);
    }

    private GetRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, MyRecipeBook.Domain.Entities.Recipe? recipe = null)
    {
        var loggedUSer = LoggedUserBuilder.Build(user);

        var mapper = MapperBuilder.Build();

        var readOnlyrepository = new RecipeReadOnlyRepositoryBuilder();

        if (recipe != null) 
        {
            readOnlyrepository.GetRecipe(user.Id, recipe);
        }

        return new GetRecipeUseCase(readOnlyrepository.Build(), loggedUSer, mapper);
    }
}
