using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebAPI.Test.InLineData;

namespace WebAPI.Test.User.Login.DoLogin;

public class DoLoginTest : MyRecipeBookClassFixture
{
    private readonly string METHOD = "login";

    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    //Instanciando a API
    public DoLoginTest(CustomWebApplicationFactory factory) : base(factory) 
    {
        _email = factory.GetEmail();
        _password = factory.GetPassword(); 
        _name = factory.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        { 
            Email = _email,
            Password = _password
        };

        var response = await DoPost(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseJson = await JsonDocument.ParseAsync(responseBody);

        responseJson.RootElement.GetProperty("name")
            .GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name);
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Invalid_Login(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();

        var response = await DoPost(METHOD, request, culture: culture) ;

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseJson = await JsonDocument.ParseAsync(responseBody);

        //Convertendo List em um EnumerableArray
        var errors = responseJson.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMenssage = ResourceErroMensage.ResourceManager.GetString("Email_Or_Password_Invalid", new CultureInfo(culture));

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMenssage));
    }

}