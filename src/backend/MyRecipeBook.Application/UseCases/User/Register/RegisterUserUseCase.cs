#region USING
using AutoMapper;
using Azure.Core;
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Communication.Responses.ErrorResponse;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
#endregion

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUserReadOnlyReapository _readOnlyRepository;
    private readonly IUnitOfWork _unityOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IMapper _mapper;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public RegisterUserUseCase(IUserWriteOnlyRepository writeOnlyRepository
        , IUserReadOnlyReapository readOnlyReapository
        , IMapper mapper
        , IPasswordEncripter passwordEncripter
        , IUnitOfWork unityOfWork
        , IAccessTokenGenerator accessTokenGenerator)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyReapository;
        _passwordEncripter = passwordEncripter;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        //Validar a request
        await Validate(request);

        var userEntity = await SaveNewUser(request);

        return MapResponse(userEntity);
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var result = await ValidateRequest(request);
        await ValidateUserExists(result, request.Email);
    }

    private async Task<FluentValidation.Results.ValidationResult> ValidateRequest(RequestRegisterUserJson request)
    {
        var result = GetValidateResult(request);

        if (!result.IsValid)
            ThrowErrorOnValidationException(result);

        return result;  
    }

    private static FluentValidation.Results.ValidationResult GetValidateResult(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();

        return validator.Validate(request);
    }

    private async Task ValidateUserExists(FluentValidation.Results.ValidationResult result, string requestEmail)
    {
        if (await UserAlreadyRegistered(requestEmail))
        {
            result = AddErrorOnValidation(result, ResourceErroMensage.User_Registred);
            ThrowErrorOnValidationException(result);
        }
    }

    private async Task<bool> UserAlreadyRegistered(string requestEmail) 
        => await _readOnlyRepository.ExistsActiveUserEmail(requestEmail);

    private static FluentValidation.Results.ValidationResult AddErrorOnValidation(FluentValidation.Results.ValidationResult result, string errorMessage, string propertyName = null)
    {
        propertyName = string.IsNullOrWhiteSpace(propertyName) ? string.Empty : propertyName;   

        result.Errors.Add(new FluentValidation.Results.ValidationFailure(propertyName, errorMessage));

        return result;
    }

    private void ThrowErrorOnValidationException(FluentValidation.Results.ValidationResult result) => throw new ErroOnValidationException(GetErrorMessage(result));

    private static List<string> GetErrorMessage(FluentValidation.Results.ValidationResult result) => result.Errors.Select(e => e.ErrorMessage).ToList();

    private async Task<MyRecipeBook.Domain.Entities.User> SaveNewUser(RequestRegisterUserJson request)
    {
        var userEntity = MapUserEntity(request);

        await _writeOnlyRepository.AddUser(userEntity);
        await _unityOfWork.Commit();

        return userEntity;
    }

    private MyRecipeBook.Domain.Entities.User MapUserEntity (RequestRegisterUserJson request)
    {
        //Mapear a request em uma entidade (Representando a classe que existe no banco de dados)
        var user = _mapper.Map<Domain.Entities.User>(request);

        //Criptografia da senha
        user.Password = GenereatePasswordEncripted(request.Password);

        user.UserIdentifier = Guid.NewGuid();

        return user;
    }
    private string GenereatePasswordEncripted(string password) => _passwordEncripter.Encripter(password);

    private ResponseRegisteredUserJson MapResponse(MyRecipeBook.Domain.Entities.User userEntity)
    {
        var response = new ResponseRegisteredUserJson
        {
            Name = userEntity.Name,
            Tokens = new ResponseTokenJson
            {
                AccessToken = _accessTokenGenerator.Generate(userEntity.UserIdentifier)
            }
        };

        return response;
    }
}
