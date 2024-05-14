using Bogus;
using CommonTestUtilities.Password;
using MyRecipeBook.Communication.Requests.UserRquest;

namespace CommonTestUtilities.Requests;

public class RequestDesactivateUserJsonBuilder
{
    public RequestDesactivateUserJson Build(int passwordLength = 10)       
    {
        return new Faker<RequestDesactivateUserJson>().RuleFor(request => request.UserPassword, PasswordGenerator.GeneratePassword(passwordLength));
    }
}
