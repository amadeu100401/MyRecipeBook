#region USING
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Communication.Responses.ErrorResponse;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;
#endregion

namespace MyRecipeBook.Application.UseCases.User.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyReapository _repository;
    private readonly IPasswordEncripter _encripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public DoLoginUseCase(IUserReadOnlyReapository repository, IPasswordEncripter encripter, IAccessTokenGenerator accessTokenGenerator)
    {
        _repository = repository;
        _encripter = encripter;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseRegisteredUserJson> Exceute(RequestLoginJson request)
    {
        await ValidateRequest(request);

        return await BuildResponseRegisteredUser(request);
    }

    private static async Task ValidateRequest(RequestLoginJson request)
    {
        var result = GetValiteResult(request);

        if (!result.IsValid) 
            throw new InvalidLoginException();
    }

    private static FluentValidation.Results.ValidationResult GetValiteResult(RequestLoginJson request)
    {
        var validator = new DoLoginValidator();

        return validator.Validate(request);
    }

    private async Task<ResponseRegisteredUserJson> BuildResponseRegisteredUser(RequestLoginJson request)
    {
        var user = await GetUserByEmailAndPassword(request.Email, request.Password);

        return CreateResponseObject(user);
    }

    private async Task<MyRecipeBook.Domain.Entities.User> GetUserByEmailAndPassword(string userEmail, string userPassword) 
        => await _repository.GetByEmailAndPassword(userEmail, GetEncriptedPassword(userPassword)) ?? throw new InvalidLoginException();

    private string GetEncriptedPassword(string password) => _encripter.Encripter(password);

    private ResponseRegisteredUserJson CreateResponseObject(MyRecipeBook.Domain.Entities.User user)
    {
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokenJson
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier)
            }
        };
    }
}
