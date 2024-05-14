using Bogus;
using CommonTestUtilities.Cryptograph;
using CommonTestUtilities.Password;
using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static (User user, string password) Build()
    {
        var password = CreatePassword();

        var user = new Faker<User>()
            .RuleFor(user => user.Id, () => 1)
            .RuleFor(user => user.Name, (f) => f.Person.FirstName)
            .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(user => user.UserIdentifier, _ => Guid.NewGuid())
            .RuleFor(user => user.Password, (f) => PasswordEncripter(password));

        return (user, password);
    }

    private static string CreatePassword() => PasswordGenerator.GeneratePassword(length: 10);

    private static string PasswordEncripter(string password) => PasswordEncripterBuilder.Build().Encripter(password);

}
