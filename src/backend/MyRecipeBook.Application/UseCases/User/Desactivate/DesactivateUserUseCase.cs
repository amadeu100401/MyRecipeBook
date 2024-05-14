#region USING
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
#endregion

namespace MyRecipeBook.Application.UseCases.User.Desactivate;

public class DesactivateUserUseCase : IDesactivateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _updateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly MyRecipeBook.Domain.Entities.User _user;

    public DesactivateUserUseCase(ILoggedUser loggedUser,
        IUserUpdateOnlyRepository updateOnlyRepository,
        IUnitOfWork unitOfWork, IPasswordEncripter passwordEncripter)
    {
        _loggedUser = loggedUser;
        _updateRepository = updateOnlyRepository;
        _passwordEncripter = passwordEncripter;
        _unitOfWork = unitOfWork;

        _user = _loggedUser.GetUser().Result;
    }

    public async Task Execute(RequestDesactivateUserJson request)
    {
        Validate(request);

        await UpdateUserStatus();
    }

    private void Validate(RequestDesactivateUserJson request)
    {
        var result = GetRequestValidate(request);

        if (!result.IsValid || !IsEqualPassword(request.UserPassword))
            ThrowException();     
    }

    private  FluentValidation.Results.ValidationResult GetRequestValidate(RequestDesactivateUserJson request)
        => new DesactiveUserValidator().Validate(request);

    private bool IsEqualPassword(string requestPassword) => _passwordEncripter.Encripter(requestPassword).Equals(_user.Password);

    private async Task UpdateUserStatus()
    {
        var userUpdated = await BuildUpdatedUserToSave();

        _updateRepository.Update(userUpdated!);

        await _unitOfWork.Commit();
    }

    private static void ThrowException() => throw new ErroOnValidationException(GetErroMessageList());

    private static List<string> GetErroMessageList() => new List<string> { ResourceErroMensage.Equal_Password };
                                                                      
    private async Task<MyRecipeBook.Domain.Entities.User> GetUserInDb() => await _updateRepository.GetById(_user.Id);

    private async Task<MyRecipeBook.Domain.Entities.User> BuildUpdatedUserToSave()
    {
        var user = await GetUserInDb();
        user.Active = false;

        return user;
    }
}
