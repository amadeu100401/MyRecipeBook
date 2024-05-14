using CommonTestUtilities.Cryptograph;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.User.Register;

public class RegisterUserUseCaseTeste
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Tokens.Should().NotBeNull();
        result.Tokens.AccessToken.Should().NotBeNull();
    }

    [Fact]
    public async Task Error_Email_Registred()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        //Informando o EMAIL
        var useCase = CreateUseCase(request.Email);

        //Salvando essa função dentro de ACT
        Func<Task> act = async () => await useCase.Execute(request);

        //Executando o método armazenado
        (await act.Should().ThrowAsync<ErroOnValidationException>())
            .Where(ex => ex.ErrorsMesages.Count == 1 && ex.ErrorsMesages.Contains(ResourceErroMensage.User_Registred));
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        //Salvando essa função dentro de ACT
        Func<Task> act = async () => await useCase.Execute(request);

        //Executando o método armazenado
        (await act.Should().ThrowAsync<ErroOnValidationException>())
            .Where(ex => ex.ErrorsMesages.Count == 1 && ex.ErrorsMesages.Contains(ResourceErroMensage.Name_Empty));
    }

    [Fact]
    public async Task Error_Email_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUseCase();

        //Salvando essa função dentro de ACT
        Func<Task> act = async () => await useCase.Execute(request);

        //Executando o método armazenado
        (await act.Should().ThrowAsync<ErroOnValidationException>())
            .Where(ex => ex.ErrorsMesages.Count == 1 && ex.ErrorsMesages.Contains(ResourceErroMensage.Email_Empty));
    }

    [Fact]
    public async Task Error_Email_Invalid()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "invalidEmail";

        var useCase = CreateUseCase();

        //Salvando essa função dentro de ACT
        Func<Task> act = async () => await useCase.Execute(request);

        //Executando o método armazenado
        (await act.Should().ThrowAsync<ErroOnValidationException>())
            .Where(ex => ex.ErrorsMesages.Count == 1 && ex.ErrorsMesages.Contains(ResourceErroMensage.Email_Invalid));
    }

    [Fact]
    public async Task Error_Password_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        var useCase = CreateUseCase();

        //Salvando essa função dentro de ACT
        Func<Task> act = async () => await useCase.Execute(request);

        //Executando o método armazenado
        (await act.Should().ThrowAsync<ErroOnValidationException>())
            .Where(ex => ex.ErrorsMesages.Count == 1 && ex.ErrorsMesages.Contains(ResourceErroMensage.Password_Empty));
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Error_Password_Lenght_Invalid(int passwordLenght)
    {
        var request = RequestRegisterUserJsonBuilder.Build(passwordLenght);

        var useCase = CreateUseCase();

        //Salvando essa função dentro de ACT
        Func<Task> act = async () => await useCase.Execute(request);

        //Executando o método armazenado
        (await act.Should().ThrowAsync<ErroOnValidationException>())
            .Where(ex => ex.ErrorsMesages.Count == 1 && ex.ErrorsMesages.Contains(ResourceErroMensage.Password_Length_Invalid));
    }

    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var unityOfWork = UnityOfWorkBuilder.Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

        if(!string.IsNullOrEmpty(email))    
            readRepositoryBuilder.ExistsActiveUserEmail(email);

        return new RegisterUserUseCase(writeRepository, readRepositoryBuilder.Build(), mapper, passwordEncripter, unityOfWork, accessTokenGenerator);
    }
}
