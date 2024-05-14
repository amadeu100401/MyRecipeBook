using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Responses.ErrorResponse;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using System.Net;

namespace MyRecipeBook.API.Filters;

//Indicando essa classe como um filtro de excecoes
public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        //Case ela seja desse tipo, receberá um tratamento diferente
        if(context.Exception is MyRecipeBookException)
        {
            HandleProjectException(context);
        }
        else 
        {
            ThrowUnknowException(context);
        }
    }

    private static void HandleProjectException(ExceptionContext context)
    {
        if (context.Exception is InvalidLoginException)
        {
            //Convertendo context.Exception como um InvalidLoginException
            var exception = context.Exception as InvalidLoginException;

            //Sempre que for um erro de login, será um Unauthorizes 
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Result = new UnauthorizedObjectResult(new ResponseErroJson(context.Exception.Message));
        }
        else if (context.Exception is ErroOnValidationException)
        {
            //Convertendo context.Exception como um ErroOnValidationException
            var exception = context.Exception as ErroOnValidationException;

            //Sempre que for um erro de validação, será um badRequest (ERRO DE VALIDAÇÃO)
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new BadRequestObjectResult(new ResponseErroJson(exception!.ErrorsMesages));
        }
    }

    private static void ThrowUnknowException(ExceptionContext context)
    {
        //Erro desconhecido (500)
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ResponseErroJson(ResourceErroMensage.Unkow_Error));
    }
}
