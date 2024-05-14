namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class InvalidLoginException : MyRecipeBookException
{
    public InvalidLoginException() : base(ResourceErroMensage.Email_Or_Password_Invalid)
    {
    }
}
