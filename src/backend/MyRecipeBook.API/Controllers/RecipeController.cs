#region USING
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.Recipe.CreateRecipe;
using MyRecipeBook.Application.UseCases.Recipe.DeleteRecipe;
using MyRecipeBook.Application.UseCases.Recipe.GetCategory;
using MyRecipeBook.Application.UseCases.Recipe.GetRecipe;
using MyRecipeBook.Application.UseCases.Recipe.GetUnitMeasure;
using MyRecipeBook.Application.UseCases.Recipe.UpdateRecipe;
using MyRecipeBook.Communication.Requests.RecipeRequest;
using MyRecipeBook.Communication.Responses;
#endregion

namespace MyRecipeBook.API.Controllers;

[AuthenticatedUser]
public class RecipeController : MyRecipeBookBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRecipe(
        [FromServices] ICreateRecipeUseCase useCase,
        [FromBody] RequestRegisterRecipe request)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecipe(
        [FromServices] IGetRecipeUseCase useCase,
        [FromQuery] Guid recipeRequest)
    {
        var result = await useCase.Execute(recipeRequest);

        return Ok(result);
    }

    [HttpGet]
    [Route("GetCategorys")]
    [ProducesResponseType(typeof(ResponseCategorysJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecipesCategorys(
        [FromServices] IGetCategoryUseCase useCase)
    {
        var result = await useCase.Execute();

        return Ok(result);
    }

    [HttpGet]
    [Route("getUnitMeasure")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnitOfMeasure(
        [FromServices] IGetUnitMeasureUseCase useCase)
    {
        var result = await useCase.Execute();
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResponseRecipeJson),StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRecipe(
        [FromServices] IUpdateRecipeUseCase useCase,
        [FromBody] RequestRegisterRecipe request, 
        [FromQuery] Guid recipeRequest)
    {
        var result = await useCase.Execute(recipeRequest, request);

        return Ok(result);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRecipe(
        [FromServices] IDeleteRecipeUseCase useCase,
        [FromQuery]Guid recipeRequest)
    {
        await useCase.Execute(recipeRequest);

        return NoContent();
    }
}
