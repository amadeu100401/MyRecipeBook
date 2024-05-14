#region USING
using CommonTestUtilities.Cryptograph;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Newtonsoft.Json;
using Xunit.Abstractions;
#endregion

namespace UseCases.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    private readonly ITestOutputHelper _output;

    public ChangePasswordUseCaseTest(ITestOutputHelper testOutput)
    {
        _output = testOutput;
    }

    [Fact]
    public async Task Success()
    {
        (var user, string password) = UserBuilder.Build();

        var request = new RequestChangePasswordJsonBuilder().Build();
        request.CurrentPassword = password;

        //_output.WriteLine(JsonConvert.SerializeObject(request));

        var useCase = CreatUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Current_Password_Empty()
    {
        (var user, string password) = UserBuilder.Build();

        var request = new RequestChangePasswordJsonBuilder().Build();
        request.CurrentPassword = string.Empty;

        var useCase = CreatUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErroOnValidationException>()
            .Where(ex => ex.ErrorsMesages.Contains(ResourceErroMensage.Password_Empty));

        user.Password.Should().NotBe(request.NewPassword);
    }

    [Fact]
    public async Task Error_New_Password_Empty()
    {
        (var user, string password) = UserBuilder.Build();

        var request = new RequestChangePasswordJsonBuilder().Build();
        request.NewPassword = string.Empty;

        var useCase = CreatUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErroOnValidationException>()
            .Where(ex => ex.ErrorsMesages.Contains(ResourceErroMensage.Password_Empty));

        user.Password.Should().NotBe(request.NewPassword);
    }

    [Fact]
    public async Task Error_New_Password_Invalid_Lenght()
    {
        (var user, string password) = UserBuilder.Build();

        var request = new RequestChangePasswordJsonBuilder().Build();
        request.NewPassword = "teste";

        var useCase = CreatUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErroOnValidationException>()
            .Where(ex => ex.ErrorsMesages.Contains(ResourceErroMensage.Password_Length_Invalid));

        user.Password.Should().NotBe(request.NewPassword);
    }

    [Fact]
    public async Task Error_New_Password_Invalid_Character()
    {
        (var user, string password) = UserBuilder.Build();

        var request = new RequestChangePasswordJsonBuilder().Build();
        request.NewPassword = "12345678";

        var useCase = CreatUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErroOnValidationException>()
            .Where(ex => ex.ErrorsMesages.Contains(ResourceErroMensage.Password_Character));

        user.Password.Should().NotBe(request.NewPassword);
    }

    private ChangePasswordUseCase CreatUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var unityOfWork = UnityOfWorkBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, repository, unityOfWork, passwordEncripter);
    }
}
