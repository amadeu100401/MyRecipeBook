using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _repository;

    public UserUpdateOnlyRepositoryBuilder() => _repository = new Mock<IUserUpdateOnlyRepository>();

    public UserUpdateOnlyRepositoryBuilder GetById(User user)
    {
        _repository.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);
        return this; //Devolver a propia instancia da classe usada pra chamar a função
    }

    public IUserUpdateOnlyRepository Build() => _repository.Object;
}
