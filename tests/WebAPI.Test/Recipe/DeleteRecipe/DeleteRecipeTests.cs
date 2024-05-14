#region USING
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Net;
using static WebAPI.Test.Utilities.GetResponseDataUtilitie;
#endregion

namespace WebAPI.Test.Recipe.DeleteRecipe;

public class DeleteRecipeTests : MyRecipeBookClassFixture
{
    private readonly string METHOD = "recipe";
    private readonly Guid _userIdentifier;
    private readonly Guid _recipeIdentifier;

    public DeleteRecipeTests(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _recipeIdentifier = factory.GetRecipeIdentifier();  
    }

    [Fact]
    private async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var parameters = await BuildRecipeRequestParamsDictionary(_recipeIdentifier);

        var response = await DoDelete(METHOD, parameters, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    private async Task Error_Invalid_Recipe_Identifier()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var parameters = await BuildRecipeRequestParamsDictionary(Guid.NewGuid());

        var response = await DoDelete(METHOD, parameters, token);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
