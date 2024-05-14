using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebAPI.Test.InLineData;

namespace WebAPI.Test.User.Register;
    
//Precisamos informar pra o .NET que essa classe é um teste de integração IClassFixture
//Cada classe vai rodar em um servidor (as classes não os métodos)
public class RegisterUserTest : MyRecipeBookClassFixture 
{
    private readonly string METHOD = "user";


    //"Instancia" da API
    public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        //Instanciando a API - JSON porque será passado dois parâmetros
        var response = await DoPost(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        //Convertendo a resposta em um dynamic object e verificar as informações
        await using var reponseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(reponseBody); //Convertendo em um documento Json, simulando o que o front receberia

        //Acessando o 'documento json', recuperando a propiedade, e recuperando o valor dessa propiedade como string
        responseData.RootElement.GetProperty("name").GetString().Should()
            .NotBeNullOrWhiteSpace().And.Be(request.Name);
    }

    [Theory]    
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPost(METHOD, request, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var reponseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(reponseBody);
         
        //Convertendo List em um EnumerableArray
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMenssage = ResourceErroMensage.ResourceManager.GetString("Name_Empty", new CultureInfo(culture)); 

        errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMenssage)) ;
    }
}
