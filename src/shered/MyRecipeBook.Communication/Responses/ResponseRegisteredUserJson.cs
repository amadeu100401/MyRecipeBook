using MyRecipeBook.Communication.Responses.ErrorResponse;

namespace MyRecipeBook.Communication.Responses;

public class ResponseRegisteredUserJson
{
    public string Name { get; set; } = string.Empty;
    public ResponseTokenJson Tokens { get; set; } = default!;
}
