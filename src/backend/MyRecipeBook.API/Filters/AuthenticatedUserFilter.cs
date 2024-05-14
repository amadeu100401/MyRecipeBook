using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Communication.Responses.ErrorResponse;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.API.Filters;

public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
{

    private readonly IAccessTokenValidator _tokenValidator;
    private readonly IUserReadOnlyReapository _repositorio;

    public AuthenticatedUserFilter(IAccessTokenValidator tokenValidator, IUserReadOnlyReapository repositorio)
    {
        _tokenValidator = tokenValidator;
        _repositorio = repositorio;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);
            var userIdentifier = _tokenValidator.ValidateAndGetUserIdentifier(token);

            var exist = await _repositorio.ExistActiveUserWithIdentifier(userIdentifier);

            if (!exist)
            {
                throw new MyRecipeBookException(ResourceErroMensage.No_Permision);
            }
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErroJson("Token expired")
            {
                TokenIsExpired = true,
            });
        }
        catch (MyRecipeBookException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErroJson(ex.Message));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(ResourceErroMensage.No_Permision);
        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        //Token na requisição
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();

        if (string.IsNullOrWhiteSpace(authentication))
        {
            throw new MyRecipeBookException(ResourceErroMensage.No_Token);
        }

        return authentication["Bearer ".Length..].Trim();
    }
}
