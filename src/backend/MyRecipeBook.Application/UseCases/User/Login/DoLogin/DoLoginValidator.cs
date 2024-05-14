using FluentValidation;
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Login.DoLogin;

public class DoLoginValidator : AbstractValidator<RequestLoginJson>
{
    public DoLoginValidator()
    {
        RuleFor(user => user).Must(user => !string.IsNullOrWhiteSpace(user.Email) || !string.IsNullOrWhiteSpace(user.Password))
            .WithMessage(ResourceErroMensage.Email_Or_Password_Invalid);
        RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceErroMensage.Email_Or_Password_Invalid);
        RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceErroMensage.Email_Or_Password_Invalid);
    }
}
