namespace MyRecipeBook.Domain.Security.Cryptography;

public interface IPasswordEncripter
{
    public string Encripter(string password);
}
