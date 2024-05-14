using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebAPI.Test;

//Precisamos informar pra o .NET que essa classe é um teste de integração IClassFixture
//Cada classe vai rodar em um servidor (as classes não os métodos)
public class MyRecipeBookClassFixture : IClassFixture<CustomWebApplicationFactory> //Servidor fornecido pela propia Microsoft 
{
    private readonly HttpClient _httpClient;

    //Instanciando a API
    public MyRecipeBookClassFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string method, Object request, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.PostAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoGet(string method, string token = "", string culture = "en", IDictionary<string, string>? parameters = null)
    {
        if(parameters != null && parameters.Count > 0)
        {
            var uriBuilder = AddQueryParameters(("localhost/"+method), token ,culture , parameters);
            return await _httpClient.GetAsync(requestUri: uriBuilder.Uri);
        }

        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.GetAsync(method);
    }

    protected async Task<HttpResponseMessage> DoPut(string method, Object request, string token = "", string culture = "en", IDictionary<string, string>? parameters = null)
    {
        if (parameters != null && parameters.Count > 0)
        {
            var uriBuilder = AddQueryParameters(("localhost/" + method), token, culture, parameters);
            return await _httpClient.PutAsJsonAsync(requestUri: uriBuilder.Uri, request);
        }

        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.PutAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoDelete(string method, IDictionary<string, string> parameters, string token = "", string culture = "en")
    {
        AuthorizeRequest(token);

        var uriBuilder = AddQueryParameters(("localHost/" + method), token, culture, parameters);

        return await _httpClient.DeleteAsync(requestUri: uriBuilder.ToString());   
    }

    private void ChangeRequestCulture(string culture)
    {
        //Modificando a linguagem padrão da requisicao
        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept - Language");

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
    }

    private void AuthorizeRequest(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private UriBuilder AddQueryParameters(string method, string token, string culture, IDictionary<string, string> parameters)
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        var uriBuilder = new UriBuilder(method);  

        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);  

        foreach ( var param in parameters ) 
        {
            query[param.Key] = param.Value;
        }

        uriBuilder.Query = query.ToString();
        return uriBuilder;
    }
}
