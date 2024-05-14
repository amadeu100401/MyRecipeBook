using Bogus;
using CommonTestUtilities.Password;
using MyRecipeBook.Communication.Requests.UserRquest;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordJsonBuilder
{
    public RequestChangePasswordJson Build(int passwordLenght = 10)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(request => request.CurrentPassword, PasswordGenerator.GeneratePassword(passwordLenght))
            .RuleFor(request => request.NewPassword, PasswordGenerator.GeneratePassword(passwordLenght));
    }
}
