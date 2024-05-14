namespace MyRecipeBook.Communication.Responses.ErrorResponse;

public class ResponseErroJson
{
    public IList<string> Errors { get; set; }

    public ResponseErroJson(IList<string> errors)
    {
        Errors = errors;
    }

    public bool TokenIsExpired { get; set; }

    public ResponseErroJson(string error)
    {
        Errors = new List<string>() { error };
    }
}
