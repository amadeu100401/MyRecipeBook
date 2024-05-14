using MyRecipeBook.Domain.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Infraestructure.Security.Cryptography;

public class Shar512Encripter : IPasswordEncripter
{
    private readonly string _additionalKey;

    public Shar512Encripter(string additionalKey)
    {
        _additionalKey = additionalKey;
    }

    public string Encripter(string password)
    {
        var newPassword = $"{password}{_additionalKey}";

        //Pegando os bytes da senha
        var bytes = Encoding.UTF8.GetBytes(newPassword);
        var hashByte = SHA512.HashData(bytes); //Precisa converter a lista de bytes em string

        return StringBytes(hashByte);
    }

    private static string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();

        foreach (byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }

        return sb.ToString();
    }
}
