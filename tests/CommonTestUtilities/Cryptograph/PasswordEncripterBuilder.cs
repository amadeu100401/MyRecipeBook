using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Infraestructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptograph;

public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build() => new Shar512Encripter("ABCTESTE1234");
}
