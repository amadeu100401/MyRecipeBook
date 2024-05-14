using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;

namespace WebAPI.Test.Recipe.GetCategorys;

public class GetCategorysTest : MyRecipeBookClassFixture
{
    private readonly string METHOD = "recipe/GetCategorys";
    private readonly Guid _userIdentifier;

    public GetCategorysTest(CustomWebApplicationFactory factory) : base(factory) => _userIdentifier = factory.GetUserIdentifier();

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var result = await DoGet(METHOD, token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
