#region USING
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Communication.Requests.UserRquest;
using System.Net;
#endregion

namespace WebAPI.Test.User.Desactive;

public class DesactiveUserSuccessTest : MyRecipeBookClassFixture
{
    private readonly string METHOD = "user/desactivate-user";
    private readonly Guid _userIdentifier;
    private readonly string _userPassword;

    public DesactiveUserSuccessTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _userPassword = factory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = BuildRequest();
        request.UserPassword = _userPassword;

        var response = await DoRequestToApi(request, token: BuildToken());

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_No_Token()
    {
        var request = BuildRequest();
        request.UserPassword = _userPassword;

        var response = await DoRequestToApi(request, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private RequestDesactivateUserJson BuildRequest() => new RequestDesactivateUserJsonBuilder().Build();

    private async Task<HttpResponseMessage> DoRequestToApi(RequestDesactivateUserJson request, string token) => await DoPut(METHOD, request, token);

    private string BuildToken() => JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
}
