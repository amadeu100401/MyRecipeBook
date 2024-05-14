namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class ErroOnValidationException : MyRecipeBookException
{
    public IList<string> ErrorsMesages { get; set; }

    public ErroOnValidationException(IList<string> erros) : base(string.Empty)
    {
        ErrorsMesages = erros;
    }
}
