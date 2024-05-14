#region USING
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;
using static WebAPI.Test.Utilities.GetResponseDataUtilitie;
#endregion

namespace WebAPI.Test.Recipe.GetRecipe;

public class GetRecipeTests : MyRecipeBookClassFixture
{
    private readonly string METHOD = "recipe";
    private readonly Guid _useIdentifier;
    private readonly Guid _recipeIdentifier;

    public GetRecipeTests(CustomWebApplicationFactory factory) : base(factory)
    {
        _useIdentifier = factory.GetUserIdentifier();
        _recipeIdentifier = factory.GetRecipeIdentifier();
    }

    [Fact]
    public async Task Success_With_Recipe()
    {
        var request = await BuildRecipeRequestParamsDictionary(_recipeIdentifier);

        var token = JwtTokenGeneratorBuilder.Build().Generate(_useIdentifier);

        var response = await DoGet(METHOD, token, parameters: request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await GetResponseData(response);

        responseData.RootElement.GetProperty("title").GetString()
            .Should().NotBeNullOrWhiteSpace();

        responseData.RootElement.GetProperty("recipeIdentifier").GetString()
            .Should().Be(_recipeIdentifier.ToString());
     }

    [Fact]
    public async Task Success_Without_Recipe()
    {
        var request = await BuildRecipeRequestParamsDictionary();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_useIdentifier);

        var response = await DoGet(METHOD, token, parameters: request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
