using Moq;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyReapository> _repository; 

    //Criando uma nova instancia do repositorio
    public UserReadOnlyRepositoryBuilder() => _repository = new Mock<IUserReadOnlyReapository>();

    public void ExistsActiveUserEmail(string email)
    {
        //Sempre que essa função for chamada, será retornado o erro de Email existente
        //Basicamente estamos dizendo que deve ser retornado sob essa condições -> Se o useCase for chamado com o EMAIL (APENAS COM O MESMO EMAIL) no parametro
        _repository.Setup(repository => repository.ExistsActiveUserEmail(email)).ReturnsAsync(true); //Async porque é uma TASK
    }

    public void GetByEmailAndPassword(MyRecipeBook.Domain.Entities.User user)
    {
        //Sempre que essa função for chamada, será retornado o erro de Email existente
        //Basicamente estamos dizendo que deve ser retornado sob essa condições -> Se o useCase for chamado com o EMAIL e a SENHA no parametro
        _repository.Setup(repository => repository.GetByEmailAndPassword(user.Email, user.Password)).ReturnsAsync(user); //Async porque é uma TASK
    }

    public IUserReadOnlyReapository Build() => _repository.Object;

}
