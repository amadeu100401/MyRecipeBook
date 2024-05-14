#region USING
using AutoMapper;
using MyRecipeBook.Communication.Requests.RecipeRequest;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
#endregion

namespace MyRecipeBook.Application.UseCases.Recipe.UpdateRecipe;

public class UpdateRecipeUseCase : IUpdateRecipeUseCase
{
    private readonly IRecipeUpdateOnlyRepository _updateRepository;
    private readonly IRecipeReadOnlyRepository _readRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unityOfWork;

    private MyRecipeBook.Domain.Entities.User _user;

    public UpdateRecipeUseCase(IRecipeUpdateOnlyRepository updateRepository, IRecipeReadOnlyRepository readRepository,
        ILoggedUser loggedUser, IMapper mapper, IUnitOfWork unityOfWork)
    {
        _updateRepository = updateRepository;
        _readRepository = readRepository;
        _loggedUser = loggedUser;
        _mapper = mapper;
        _unityOfWork = unityOfWork;

        _user = GetLoggedUser().Result;
    }

    public async Task<ResponseRecipeJson> Execute(Guid recipeIdentifier, RequestRegisterRecipe request)
    {
        await ValidateRequest(request);

        var recipe = await UpdateRecipe(recipeIdentifier, request);

        return MapResponse(recipe);
    }

    private static async Task ValidateRequest(RequestRegisterRecipe request)
    {
        var result = CreateValidate(request);

        if (!isValidRequest(result))
        {
            throw new ErroOnValidationException(GetErrorMessage(result.Errors));
        }
    }

    private static FluentValidation.Results.ValidationResult CreateValidate(RequestRegisterRecipe request) => new UpdateRecipeValidator().Validate(request);

    private static bool isValidRequest(FluentValidation.Results.ValidationResult result) => result.IsValid;

    private static List<string> GetErrorMessage(List<FluentValidation.Results.ValidationFailure> errors) => errors.Select(x => x.ErrorMessage).ToList();

    private static List<string> GetErrorMessage(string message) => new List<string> { message };

    private async Task<MyRecipeBook.Domain.Entities.Recipe> UpdateRecipe(Guid recipeIdentifier, RequestRegisterRecipe request)
    {
        if (await isValidUpdate(recipeIdentifier))
        {
            var recipe = MapRecipe(recipeIdentifier, request);

            await _updateRepository.Update(recipe);
            await _unityOfWork.Commit();  

            return recipe;
        }
        else
        {
            throw new ErroOnValidationException(GetErrorMessage(ResourceErroMensage.No_Recipe));
        }     
    }

    private async Task<bool> isValidUpdate(Guid recipeIdentifier)
    {
        return await ExistsRecipe(recipeIdentifier);
    }

    private async Task<bool> ExistsRecipe(Guid recipeIdentifier)
    {
        return await _readRepository.ExistsRecipe(_user.Id, recipeIdentifier);
    }

    private async Task<MyRecipeBook.Domain.Entities.User> GetLoggedUser() => await _loggedUser.GetUser();

    private MyRecipeBook.Domain.Entities.Recipe MapRecipe(Guid recipeIdentifier, RequestRegisterRecipe request)
    {
        var recipeMapped = _mapper.Map<MyRecipeBook.Domain.Entities.Recipe>(request);
        recipeMapped.RecipeIdentifier = recipeIdentifier;
        recipeMapped.UserId = _user.Id;
        return recipeMapped;
    }

    private MyRecipeBook.Communication.Responses.ResponseRecipeJson MapResponse(MyRecipeBook.Domain.Entities.Recipe recipe) => _mapper.Map<ResponseRecipeJson>(recipe);
}
