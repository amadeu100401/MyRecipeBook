#region USING
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebAPI.Test.InLineData;
#endregion

namespace WebAPI.Test.Recipe.CreateRecipe;

public class RegisterRecipeTest : MyRecipeBookClassFixture
{
    private readonly string METHOD = "recipe";
    private readonly Guid _userIdentifier;

    public RegisterRecipeTest(CustomWebApplicationFactory factory) : base(factory) => _userIdentifier = factory.GetUserIdentifier();

    [Fact]
    public async Task Success()
    {
        var request = new RequestRegisterRecipeJsonBuilder().Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        //Pegando o conteudo da requisição
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        //Recuperando as informações do json
        responseData.RootElement.GetProperty("title").GetString()
            .Should().NotBeNullOrWhiteSpace().And.Be(request.Title);
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Erro_No_Token(string culture)
    {
        var request = new RequestRegisterRecipeJsonBuilder().Build();

        var response = await DoPost(METHOD, request, "", culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        //Pegando o conteudo da requisição
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var error = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMesssage = ResourceErroMensage.ResourceManager.GetString("No_Token", new CultureInfo(culture));

        //Recuperando as informações do json
        error.Should().ContainSingle().And.Contain(ex => ex.GetString()!.Equals(expectedMesssage));
    }


    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Erro_Invalid_Token(string culture)
    {
        var request = new RequestRegisterRecipeJsonBuilder().Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPost(METHOD, request, token, culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        //Pegando o conteudo da requisição
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var error = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErroMensage.ResourceManager.GetString("No_Permision", new CultureInfo(culture));

        //Recuperando as informações do json
        error.Should().ContainSingle().And.Contain(ex => ex.GetString()!.Equals(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Erro_Empty_Title(string culture)
    {
        var request = new RequestRegisterRecipeJsonBuilder().Build();
        request.Title = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(METHOD, request, token, culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        //Pegando o conteudo da requisição
        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var error = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErroMensage.ResourceManager.GetString("Title_Empty", new CultureInfo(culture));

        //Recuperando as informações do json
        error.Should().ContainSingle().And.Contain(ex => ex.GetString()!.Equals(expectedMessage));
    }
}
