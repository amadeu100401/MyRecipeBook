using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebAPI.Test.InLineData;

namespace WebAPI.Test.User.Update;

public class UpdateUserTest : MyRecipeBookClassFixture
{
    private readonly string METHOD = "user";

    private readonly Guid _userIdentifier;

    //"Instanciando" a API
    public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory) 
    {
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var result = await DoPut(METHOD, request, token, culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var responseDataJson = await JsonDocument.ParseAsync(responseBody);

        var errors = responseDataJson.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMenssage = ResourceErroMensage.ResourceManager.GetString("Name_Empty", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMenssage));
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Empty_Email(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var result = await DoPut(METHOD, request, token, culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var responseDataJson = await JsonDocument.ParseAsync(responseBody);

        var errors = responseDataJson.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMenssage = ResourceErroMensage.ResourceManager.GetString("Email_Empty", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(e => e.GetString()!.Equals(expectedMenssage));
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_No_Token(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await DoPut(METHOD, request, culture);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Token_With_User_Not_Found(string culture)       
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var result = await DoPut(METHOD, request, culture);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
