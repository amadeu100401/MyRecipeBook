using System.Text.Json;

namespace WebAPI.Test.Utilities;

public static class GetResponseDataUtilitie
{
    public static async Task<JsonDocument> GetResponseData(HttpResponseMessage response)
    {
        var responseBody = await GetResponseBody(response);

        return await JsonDocument.ParseAsync(responseBody);
    }

    private static async Task<Stream> GetResponseBody(HttpResponseMessage response)
    {
        return await response.Content.ReadAsStreamAsync();
    }

    public static async Task <IDictionary<string, string>> BuildRecipeRequestParamsDictionary(Guid? recipeIdentifier = null)
    {
        if(recipeIdentifier is null)
            recipeIdentifier = Guid.NewGuid();  

        IDictionary<string, string> _params = new Dictionary<string, string>();
        _params["recipeRequest"] = recipeIdentifier!.ToString();

        return _params;
    }
}
