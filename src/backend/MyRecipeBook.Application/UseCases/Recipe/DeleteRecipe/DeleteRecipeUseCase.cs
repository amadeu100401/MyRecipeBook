#region USING
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
#endregion

namespace MyRecipeBook.Application.UseCases.Recipe.DeleteRecipe;

public class DeleteRecipeUseCase : IDeleteRecipeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _readRepository;
    private readonly IRecipeDeleteOnlyRepository _deleteRepository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly MyRecipeBook.Domain.Entities.User _user;

    public DeleteRecipeUseCase(ILoggedUser loggedUser,IRecipeReadOnlyRepository recipeReadOnlyRepository,
        IRecipeDeleteOnlyRepository recipeDeleteOnlyRepository, IUnitOfWork unityOfWork)
    {
        _loggedUser = loggedUser;   
        _readRepository = recipeReadOnlyRepository;
        _deleteRepository = recipeDeleteOnlyRepository;
        _unityOfWork = unityOfWork;
        _user = GetLoggedUser().Result;
    }

    public async Task Execute(Guid recipeIdentifier) => await DeleteRecipe(recipeIdentifier);

    private async Task DeleteRecipe(Guid recipeIdentifier)
    {
        if (await IsValidDelete(recipeIdentifier))
        {
            await _deleteRepository.Delete(_user.Id, recipeIdentifier);
            await _unityOfWork.Commit();
        } 
        else
            throw new ErroOnValidationException(GetErrorMessage());
    }

    private async Task<bool> IsValidDelete(Guid recipeIdentifier) => await ExisteRecipeWithIdentifier(recipeIdentifier); 

    private async Task<bool> ExisteRecipeWithIdentifier(Guid recipeIdentifier) => await _readRepository.ExistsRecipe(_user.Id, recipeIdentifier);

    private List<string> GetErrorMessage() => new List<string>() { ResourceErroMensage.No_Recipe };

    private async Task<MyRecipeBook.Domain.Entities.User> GetLoggedUser() => await _loggedUser.GetUser();
}
