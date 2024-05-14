using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.User.Login.DoLogin;

public interface IDoLoginUseCase
{
    public Task<ResponseRegisteredUserJson> Exceute(RequestLoginJson request);
}
