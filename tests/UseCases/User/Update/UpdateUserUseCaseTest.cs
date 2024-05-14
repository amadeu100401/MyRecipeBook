using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.User.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUserCase(user);

        Func<Task> act = async () => await useCase.Excute(request);

        await act.Should().NotThrowAsync();

        user.Name.Should().Be(request.Name); //O user possui uma referencia dentro do useCase, que quando este é alterado, também afeta no user aqui
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUserCase(user);

        Func<Task> act = async () => await useCase.Excute(request);

        await act.Should().ThrowAsync<ErroOnValidationException>()
            .Where(e => e.ErrorsMesages.Contains(ResourceErroMensage.Name_Empty));

        user.Name.Should().NotBe(request.Name); //O user possui uma referencia dentro do useCase, que quando este é alterado, também afeta no user aqui
        user.Email.Should().NotBe(request.Email);
    }

    [Fact]
    public async Task Error_Email_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUserCase(user);

        Func<Task> act = async () => await useCase.Excute(request);

        await act.Should().ThrowAsync<ErroOnValidationException>()
            .Where(e => e.ErrorsMesages.Contains(ResourceErroMensage.Email_Empty));

        user.Name.Should().NotBe(request.Name); //O user possui uma referencia dentro do useCase, que quando este é alterado, também afeta no user aqui
        user.Email.Should().NotBe(request.Email);
    }

    [Fact]
    public async Task Error_Email_Already_Registred()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = CreateUserCase(user, request.Email);

        Func<Task> act = async () => await useCase.Excute(request);

        await act.Should().ThrowAsync<ErroOnValidationException>()
            .Where(e => e.ErrorsMesages.Contains(ResourceErroMensage.User_Registred));

        user.Name.Should().NotBe(request.Name); //O user possui uma referencia dentro do useCase, que quando este é alterado, também afeta no user aqui
        user.Email.Should().NotBe(request.Email);
    }

    private static UpdateUserUseCase CreateUserCase(MyRecipeBook.Domain.Entities.User user, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateOnlyRespository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var unityOfWork = UnityOfWorkBuilder.Build();

        var readRepository = new UserReadOnlyRepositoryBuilder();
        if (!string.IsNullOrEmpty(email))
            readRepository.ExistsActiveUserEmail(email!);

        return new UpdateUserUseCase(loggedUser, readRepository.Build(), updateOnlyRespository, unityOfWork);
    }
}
