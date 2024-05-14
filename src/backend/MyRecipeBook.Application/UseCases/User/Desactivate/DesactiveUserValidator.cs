using FluentValidation;
using MyRecipeBook.Application.SheredValidator;
using MyRecipeBook.Communication.Requests.UserRquest;

namespace MyRecipeBook.Application.UseCases.User.Desactivate;

public class DesactiveUserValidator : AbstractValidator<RequestDesactivateUserJson>
{
    public DesactiveUserValidator()
    {
        RuleFor(request => request.UserPassword).SetValidator(new PasswordValidator<RequestDesactivateUserJson>());
    }
}
