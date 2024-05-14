namespace MyRecipeBook.Exceptions.ExceptionsBase;

//Colocando essa classe como exceção 
public class MyRecipeBookException : SystemException
{
    public MyRecipeBookException(string message) : base(message) { }
}
