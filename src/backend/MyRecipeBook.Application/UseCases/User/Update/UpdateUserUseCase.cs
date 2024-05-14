#region USING
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
#endregion

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserReadOnlyReapository _readRepository;
    private readonly IUserUpdateOnlyRepository _updateRepository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly MyRecipeBook.Domain.Entities.User _user;

    public UpdateUserUseCase(ILoggedUser loggedUser, IUserReadOnlyReapository readRepository
        , IUserUpdateOnlyRepository updateRepository, IUnitOfWork unityOfWork)
    {
        _loggedUser = loggedUser;
        _readRepository = readRepository;   
        _updateRepository = updateRepository;   
        _unityOfWork = unityOfWork;

        _user = _loggedUser.GetUser().Result;
    }

    public async Task Excute(RequestUpdateUserJson request)
    {
        await Validate(request);

        await SaveChanges(request);
    }

    private async Task Validate(RequestUpdateUserJson request)
    {
        var result = ValidateRequest(request);

        result = await ValidateEmailReistered(result, request.Email);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErroOnValidationException(errorMessages);
        }
    }

    private FluentValidation.Results.ValidationResult ValidateRequest(RequestUpdateUserJson request) => GetValidateResult(request);

    private FluentValidation.Results.ValidationResult GetValidateResult(RequestUpdateUserJson request)
    {
        var validator = new UpdateUserValidator();

        return validator.Validate(request); 
    }

    private async Task<FluentValidation.Results.ValidationResult> ValidateEmailReistered(FluentValidation.Results.ValidationResult result, string requestEmail) 
    {
        if (!RequestEmailEqualsCurrentUserEmail(requestEmail))
        {
            var userExist = await EmailAlreadyRegistered(requestEmail);
            if (userExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceErroMensage.User_Registred));
        }

        return result;
    }

    private bool RequestEmailEqualsCurrentUserEmail(string requestEmail) => _user.Email.Equals(requestEmail);

    private async Task<bool> EmailAlreadyRegistered(string requestEmail) => await _readRepository.ExistsActiveUserEmail(requestEmail);

    private async Task SaveChanges(RequestUpdateUserJson request)
    {
        var user = await GetUserRegistered();

        user.Name = request.Name;
        user.Email = request.Email;

        _updateRepository.Update(user);

        await _unityOfWork.Commit();
    }

    private async Task<MyRecipeBook.Domain.Entities.User> GetUserRegistered() =>  await _updateRepository.GetById(_user.Id);
}
