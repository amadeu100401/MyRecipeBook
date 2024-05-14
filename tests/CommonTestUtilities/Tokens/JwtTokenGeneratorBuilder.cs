using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infraestructure.Security.Tokens.Access.Generator;

namespace CommonTestUtilities.Tokens;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey:"ttttttttttttttttttttttttttttttttt");
}
