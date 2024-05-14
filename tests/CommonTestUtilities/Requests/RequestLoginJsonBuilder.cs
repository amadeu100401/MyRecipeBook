using Bogus;
using MyRecipeBook.Communication.Requests.UserRquest;

namespace CommonTestUtilities.Requests;

public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build(int passwordLengh = 10)
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(user => user.Email, (f) => f.Internet.Email())
            .RuleFor(user => user.Password, (f) => f.Internet.Password(passwordLengh));
    }
}
