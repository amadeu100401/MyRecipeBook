#region USIGN
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;
using static WebAPI.Test.Utilities.GetResponseDataUtilitie;
#endregion

namespace WebAPI.Test.Recipe.UpdateRecipeTests;

public class UpdateRecipeTests : MyRecipeBookClassFixture
{
    private readonly string METHOD = "recipe";
    private readonly Guid _userIdentifier;
    private readonly Guid _recipeIdentifier;

    public UpdateRecipeTests(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _recipeIdentifier = factory.GetRecipeIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var request = new RequestRegisterRecipeJsonBuilder().Build();

        var response = await DoPut(METHOD, request: request, token: token, parameters: await BuildRecipeRequestParamsDictionary(_recipeIdentifier));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await GetResponseData(response);

        responseData.RootElement.GetProperty("title").GetString()
            .Should().NotBeNullOrWhiteSpace().And.Be(request.Title);
    }

    [Fact]
    public async Task Error_Recipe_Identifier_Not_Found()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var request = new RequestRegisterRecipeJsonBuilder().Build();

        var response = await DoPut(METHOD, request: request, token: token, parameters: await BuildRecipeRequestParamsDictionary(Guid.NewGuid()));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Error_Request_Title_Empty()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var request = new RequestRegisterRecipeJsonBuilder().Build();
        request.Title = string.Empty;   

        var response = await DoPut(METHOD, request: request, token: token, parameters: await BuildRecipeRequestParamsDictionary(_recipeIdentifier));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
