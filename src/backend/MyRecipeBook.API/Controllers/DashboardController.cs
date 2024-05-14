#region USING
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.Dashboard;
using MyRecipeBook.Communication.Requests.Dashboard;
using MyRecipeBook.Communication.Responses.Dashboard;
#endregion

namespace MyRecipeBook.API.Controllers;

[AuthenticatedUser]
public class DashboardController : MyRecipeBookBaseController
{

    [HttpGet]
    [ProducesResponseType(typeof(ResponseDashboardJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecipe(
        [FromServices] IGetDashboardUseCase useCase,
        [FromQuery] RequestDashboardJson request)
    {
        var result = await useCase.Execute(request);

        return Ok(result);
    }
}
