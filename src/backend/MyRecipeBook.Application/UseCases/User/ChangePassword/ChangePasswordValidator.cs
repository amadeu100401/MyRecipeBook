using FluentValidation;
using MyRecipeBook.Application.SheredValidator;
using MyRecipeBook.Communication.Requests.UserRquest;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(password => password.CurrentPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
        RuleFor(password => password.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}
