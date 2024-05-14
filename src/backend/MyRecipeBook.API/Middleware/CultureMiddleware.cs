using System.Globalization;

namespace MyRecipeBook.API.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        //Pegando todas as culturas que o .NET suporta
        var surpportedLanguagens = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

        //Recuperar a requisição enviada na requisição
        var requestedCulture = string.Empty;

        if (!string.IsNullOrWhiteSpace(context.Request.Headers.AcceptLanguage))
            requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault().Split(',')[0];

        var cultureInfo = new CultureInfo("en");

        //Verificando se existe o culture na requisição e se esse existe nas culturas suportadas
        if(!string.IsNullOrWhiteSpace(requestedCulture) 
            && surpportedLanguagens.Exists(c => c.Name.Equals(requestedCulture)))
        {
            cultureInfo = new CultureInfo(requestedCulture);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);   //Indicando que o fluxo pode continuar 
    }
}
