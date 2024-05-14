using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyRecipeBook.Infraestructure.Security.Tokens.Access.Generator;

public abstract class JwtTokenHandler
{
    protected SymmetricSecurityKey SecutiryKey(string signingKey)
    {
        var bytes = Encoding.UTF8.GetBytes(signingKey);

        return new SymmetricSecurityKey(bytes);
    }
}
