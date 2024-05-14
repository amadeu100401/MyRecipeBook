#region USING
using AutoMapper;
using MyRecipeBook.Communication.Requests.RecipeRequest;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions.ExceptionsBase;
#endregion

namespace MyRecipeBook.Application.UseCases.Recipe.CreateRecipe;

public class CreateRecipeUseCase : ICreateRecipeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeWriteOnlyRepository _recipeWriteRepository;
    private readonly IUserReadOnlyReapository _userReadOnlyRepository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly IMapper _mapper;
    private readonly MyRecipeBook.Domain.Entities.User _user;

    public CreateRecipeUseCase(ILoggedUser loggedUser, IRecipeWriteOnlyRepository recipeWriteOnly
        ,IUserReadOnlyReapository userReadOnlyReapository, IMapper mapper, IUnitOfWork unityOfWork)
    {
        _loggedUser = loggedUser;
        _recipeWriteRepository = recipeWriteOnly;
        _userReadOnlyRepository = userReadOnlyReapository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;

        _user = _loggedUser.GetUser().Result; 
    }

    public async Task<ResponseRecipeJson> Execute(RequestRegisterRecipe request)
    {
        await ValidateRequest(request);

        return await SaveRecipe(request);
    }

    private async Task ValidateRequest(RequestRegisterRecipe request)
    {
        var result = ValidateResult(request);   

        if (!result.IsValid)
        {
            var errorMessage = result.Errors.Select(ex => ex.ErrorMessage).ToList();
            throw new ErroOnValidationException(errorMessage);
        }
    }

    private static FluentValidation.Results.ValidationResult ValidateResult(RequestRegisterRecipe request)
    {
        var validator = new CreateRecipeValidator();

        return validator.Validate(request);
    }

    private async Task<ResponseRecipeJson> SaveRecipe(RequestRegisterRecipe request)
    {
        var recipeEntity = MapRecipeEntity(request);

        await _recipeWriteRepository.AddRecipe(recipeEntity);
        await _unityOfWork.Commit();

        return MapResponse(recipeEntity);
    }

    private ResponseRecipeJson MapResponse(MyRecipeBook.Domain.Entities.Recipe recipe) => _mapper.Map<ResponseRecipeJson>(recipe);

    private MyRecipeBook.Domain.Entities.Recipe MapRecipeEntity(RequestRegisterRecipe request)
    {
        var recipe = _mapper.Map<Domain.Entities.Recipe>(request);

        recipe.UserId = _user.Id;

        recipe.RecipeIdentifier = Guid.NewGuid();
        recipe.Ingredients.ForEach(ingredient => ingredient.IngredientsIdentifier = Guid.NewGuid());

        return recipe;
    }
}
