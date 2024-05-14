#region USING
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Desactivate;
using MyRecipeBook.Application.UseCases.User.Profil;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Communication.Responses.ErrorResponse;
#endregion 

namespace MyRecipeBook.API.Controllers;

public class UserController : MyRecipeBookBaseController
{
    [HttpPost] //Tipo do verbo HTTP
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)] //Informando o que será retornado e qual o código esperado
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase, //Pega dos serviços de injeção de depêndencia 
        [FromBody] RequestRegisterUserJson request) //Vai pegar as infomrações do Body do request
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfile), StatusCodes.Status200OK)]
    [AuthenticatedUser] //Ele so poderá acessar esse endpoint caso esteja autorizado
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var result = await useCase.Execute();

        return Ok(result);   
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErroJson), StatusCodes.Status400BadRequest)]
    [AuthenticatedUser]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateUserUseCase useCase,
        [FromBody] RequestUpdateUserJson request)
    {
        await useCase.Excute(request);

        return NoContent();
    }

    [HttpPut("change-password")] //Como não pode haver dois endpoints repetidos com o mesmo verbo, foi preciso especificar esse aqui
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErroJson), StatusCodes.Status400BadRequest)]
    [AuthenticatedUser]
    public async Task<IActionResult> ChangePassword(
        [FromServices] IChangePasswordUseCase useCase,
        [FromBody] RequestChangePasswordJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    [HttpPut("desactivate-user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [AuthenticatedUser]
    public async Task<IActionResult> DesactivateUserAccount(
        [FromServices] IDesactivateUserUseCase useCase,
        [FromBody] RequestDesactivateUserJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
}
