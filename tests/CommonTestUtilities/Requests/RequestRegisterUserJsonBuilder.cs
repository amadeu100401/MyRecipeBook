using Bogus;
using CommonTestUtilities.Password;
using MyRecipeBook.Communication.Requests.UserRquest;

namespace CommonTestUtilities.Requests;

public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build(int passwordLength = 10)
    {
        var faker = new Faker();

        // Gerar uma senha aleatória com pelo menos uma letra maiúscula e um dígito
        string password = PasswordGenerator.GeneratePassword(passwordLength);

        return new Faker<RequestRegisterUserJson>()
            .RuleFor(user => user.Name, (f) => f.Person.FirstName)
            .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(user => user.Password, password);
    }
}
