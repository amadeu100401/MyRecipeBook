using MyRecipeBook.Communication.Requests.UserRquest;

namespace MyRecipeBook.Application.UseCases.User.Desactivate;

public interface IDesactivateUserUseCase
{
    public Task Execute(RequestDesactivateUserJson request);
}
