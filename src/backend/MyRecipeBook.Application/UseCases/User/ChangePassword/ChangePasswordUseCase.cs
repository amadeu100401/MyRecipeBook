#region USING
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
#endregion

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _updateRepository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly MyRecipeBook.Domain.Entities.User _user;

    public ChangePasswordUseCase(ILoggedUser loggedUser, IUserUpdateOnlyRepository userUpdateOnly, 
        IUnitOfWork unityOfWork, IPasswordEncripter passwordEncripter)
    {
        _loggedUser = loggedUser;
        _updateRepository = userUpdateOnly;
        _unityOfWork = unityOfWork;
        _passwordEncripter = passwordEncripter;

        _user = loggedUser.GetUser().Result;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        await isValidadeToSave(request);

        await SaveChanges(request);
    }

    private async Task isValidadeToSave(RequestChangePasswordJson request)
    {
        var result = await GetRequestValidation(request);

        var newPasswordEncripted = await GetEncriptedPassword(request.NewPassword);

        if (newPasswordEqualsCurrentPassword(newPasswordEncripted))
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty,ResourceErroMensage.Equal_Password));

        if(!result.IsValid)
            throw new ErroOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }

    private static async Task<FluentValidation.Results.ValidationResult> GetRequestValidation(RequestChangePasswordJson request) 
        => new ChangePasswordValidator().Validate(request);

    private async Task<string> GetEncriptedPassword(string password) => _passwordEncripter.Encripter(password);

    private bool newPasswordEqualsCurrentPassword(string newPassword) => newPassword.Equals(_user.Password);

    private async Task SaveChanges(RequestChangePasswordJson request)
    {
        var user = await BuildUserToSave(request.NewPassword);

        _updateRepository.Update(user);

        await _unityOfWork.Commit();
    }

    private async Task<MyRecipeBook.Domain.Entities.User> BuildUserToSave(string newPassword)
    {
        var user = await GetUserById();
        user.Password = await GetEncriptedPassword(newPassword);
        return user;
    }

    private async Task<MyRecipeBook.Domain.Entities.User> GetUserById() => await _updateRepository.GetById(_user.Id);

}
