using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infraestructure.Security.Tokens.Access.Generator;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyRecipeBook.Infraestructure.Security.Tokens.Access.Validator;

public class JwtTokenValidator : JwtTokenHandler , IAccessTokenValidator
{
    private readonly string _signingKey;

    public JwtTokenValidator(string signingKey) => _signingKey = signingKey;

    public Guid ValidateAndGetUserIdentifier(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false, //Verificar quem criou o token 
            IssuerSigningKey = SecutiryKey(_signingKey),
            //Validar o tempo de expiração
            ClockSkew = new TimeSpan(0),
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        //Valida o token
        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

        //Obtem o GUID 
        var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        return Guid.Parse(userIdentifier);  
    }
}
