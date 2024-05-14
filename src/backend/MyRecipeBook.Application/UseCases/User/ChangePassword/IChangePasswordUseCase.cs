using MyRecipeBook.Communication.Requests.UserRquest;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    public Task Execute(RequestChangePasswordJson request);
}
