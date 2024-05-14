using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.User.Profil;

public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfile> Execute();
}
