using MyRecipeBook.Communication.Requests.UserRquest;

namespace MyRecipeBook.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    public Task Excute(RequestUpdateUserJson requet);
}
