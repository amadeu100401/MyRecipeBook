using FluentValidation;
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceErroMensage.Name_Empty);
        RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceErroMensage.Email_Empty);

        When(request => !string.IsNullOrEmpty(request.Email), () =>
        {
            RuleFor(request => request.Email).EmailAddress().WithMessage(ResourceErroMensage.Email_Invalid);
        });
    }

}
