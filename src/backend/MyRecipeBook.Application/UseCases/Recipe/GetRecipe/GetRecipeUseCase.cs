#region USING
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
#endregion

namespace MyRecipeBook.Application.UseCases.Recipe.GetRecipe;

public class GetRecipeUseCase : IGetRecipeUseCase
{
    private readonly IRecipeReadOnlyRepository _readOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetRecipeUseCase(IRecipeReadOnlyRepository readOnlyRepository, ILoggedUser loggedUser, IMapper mapper)
    {
        _readOnlyRepository = readOnlyRepository;
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<ResponseRecipeJson> Execute(Guid recipeIdentifier)
    {
        var userRecipeList = await GetUsersRecipe();

        return Filter(recipeIdentifier, userRecipeList);
    }

    private async Task<List<MyRecipeBook.Domain.Entities.Recipe>> GetUsersRecipe()
    {
        var userLogged = await GetLoggedUser();

        return await _readOnlyRepository.GetRecipe(userLogged.Id);
    }
    private async Task<MyRecipeBook.Domain.Entities.User> GetLoggedUser()
    {
        return await _loggedUser.GetUser();
    }

    private MyRecipeBook.Communication.Responses.ResponseRecipeJson Filter(Guid recipeIdentifier, List<MyRecipeBook.Domain.Entities.Recipe> recipeList)
    {
        if (recipeList.IsNullOrEmpty())
            return null;

        var recipeFiltered = FilterByRecipIdentifier(recipeList, recipeIdentifier);

        return ConvertDomainToResponse(recipeFiltered);
    }

    private MyRecipeBook.Domain.Entities.Recipe FilterByRecipIdentifier(List<MyRecipeBook.Domain.Entities.Recipe> noFilteredRecipeList, 
        Guid recipeIdentifierRequest)
    {
        return noFilteredRecipeList.FirstOrDefault(recipe => recipe.RecipeIdentifier == recipeIdentifierRequest); ;
    }

    private MyRecipeBook.Communication.Responses.ResponseRecipeJson ConvertDomainToResponse(MyRecipeBook.Domain.Entities.Recipe recipeDomain)
    {
        return _mapper.Map<MyRecipeBook.Communication.Responses.ResponseRecipeJson>(recipeDomain);
    }
}
