using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;

namespace WebAPI.Test.User.ChangePassword;

public class ChangePasswordTest : MyRecipeBookClassFixture
{
    private readonly string METHOD = "user/change-password";

    private readonly string _password;
    private readonly Guid _userIdentifier;

    public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _password = factory.GetPassword();
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var request = new RequestChangePasswordJsonBuilder().Build();
        request.CurrentPassword = _password;

        var response = await DoPut(METHOD, request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);        
    }

    [Fact]
    public async Task Error_Current_Password_Empty()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var request = new RequestChangePasswordJsonBuilder().Build();
        request.CurrentPassword = string.Empty;

        var response = await DoPut(METHOD, request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Error_New_Password_Empty()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var request = new RequestChangePasswordJsonBuilder().Build();
        request.NewPassword = string.Empty;

        var response = await DoPut(METHOD, request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Error_New_Password_Invalid()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var request = new RequestChangePasswordJsonBuilder().Build();
        request.NewPassword = "teste";

        var response = await DoPut(METHOD, request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
