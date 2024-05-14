using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.User.Login.DoLogin;
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Communication.Responses.ErrorResponse;

namespace MyRecipeBook.API.Controllers
{
    public class LoginController : MyRecipeBookBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErroJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase,
            [FromBody] RequestLoginJson request)
        {
            var response = await useCase.Exceute(request);

            return Ok(response);
        }
    }
}
