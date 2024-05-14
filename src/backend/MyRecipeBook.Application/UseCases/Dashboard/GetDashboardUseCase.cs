#region USING
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Requests.Dashboard;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Communication.Responses.Dashboard;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
#endregion

namespace MyRecipeBook.Application.UseCases.Dashboard;

public class GetDashboardUseCase : IGetDashboardUseCase
{
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetDashboardUseCase(IRecipeReadOnlyRepository repository, ILoggedUser loggedUser, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseDashboardJson> Execute(RequestDashboardJson request)
    {
        var userRecipes = await GetUsersRecipe();

        return new ResponseDashboardJson()
        { 
            UsersRecipes = ApplyFilters(request, userRecipes)
        };
    }

    private async Task<MyRecipeBook.Domain.Entities.User> GetLoggedUser()
    {
        return await _loggedUser.GetUser();
    }

    private async Task<List<MyRecipeBook.Domain.Entities.Recipe>> GetUsersRecipe()
    {
        var loggedUser = await GetLoggedUser();

        return await _repository.GetRecipe(loggedUser.Id);
    }

    private List<ResponseRecipeJson> ApplyFilters(RequestDashboardJson request, List<MyRecipeBook.Domain.Entities.Recipe> noFilterRecipe)
    {
        if (!isValidApplyFiltering(noFilterRecipe))
            return new List<ResponseRecipeJson>();

        var filteredRecipes = noFilterRecipe;

        if (isValidFilterByTitle(request.TitleOrIngredientName))
            filteredRecipes = FilterByTitle(request.TitleOrIngredientName, filteredRecipes);
        else if (isValidFilterByMaxTime(request.MaxTime))
            filteredRecipes = FilterByMaxTime((double)request.MaxTime!, filteredRecipes);
        else if (isValidFilterByCategory(request.Category))
            filteredRecipes = FilterByCategory((MyRecipeBook.Communication.Enum.Category)request.Category, filteredRecipes);

        return MappingToResponse(filteredRecipes);
    }

    private static bool isValidApplyFiltering(List<MyRecipeBook.Domain.Entities.Recipe> noFilterRecipe)
    {
        return !noFilterRecipe.IsNullOrEmpty() && noFilterRecipe.Count > 0;
    }

    private static bool isValidFilterByTitle(string titleOrName)
    {
        return !string.IsNullOrWhiteSpace(titleOrName);
    }

    private static List<MyRecipeBook.Domain.Entities.Recipe> FilterByTitle(string titleOrName, List<MyRecipeBook.Domain.Entities.Recipe> noFilteredRecipes ) 
    {
        return noFilteredRecipes.Where(recipe => recipe.Title.CompareStrings(titleOrName) 
        || recipe.Ingredients.Any(ingredients => ingredients.Name.CompareStrings(titleOrName))).ToList();
    }

    private static bool isValidFilterByMaxTime(double? maxTime)
    {
        return maxTime != null && maxTime.HasValue;
    }

    private static List<MyRecipeBook.Domain.Entities.Recipe> FilterByMaxTime(double maxTime, List<MyRecipeBook.Domain.Entities.Recipe> noFilteredRecipes)
    {
        return noFilteredRecipes.Where(recipe => recipe.TimePreparationMinutes <= maxTime).ToList();
    }

    private static bool isValidFilterByCategory(MyRecipeBook.Communication.Enum.Category? category)
    {
        if (category == null) 
            return false;

        return Enum.IsDefined((MyRecipeBook.Communication.Enum.Category)category);
    }

    private static List<MyRecipeBook.Domain.Entities.Recipe> FilterByCategory(MyRecipeBook.Communication.Enum.Category category,
        List<MyRecipeBook.Domain.Entities.Recipe> noFilteredRecipes)
    {
        return noFilteredRecipes.Where(recipe => recipe.Category.IsEqualTo(category, recipe.Category)).ToList();
    }

    private List<ResponseRecipeJson> MappingToResponse(List<MyRecipeBook.Domain.Entities.Recipe> filteredRecipes)
    {
        return _mapper.Map<List<ResponseRecipeJson>>(filteredRecipes);
    }

}
