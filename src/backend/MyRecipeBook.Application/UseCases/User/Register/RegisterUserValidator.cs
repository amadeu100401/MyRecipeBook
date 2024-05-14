using FluentValidation;
using MyRecipeBook.Application.SheredValidator;
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson> //Imformando que isso é um validator para essa classe
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErroMensage.Name_Empty);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceErroMensage.Email_Empty);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceErroMensage.Email_Invalid);
        });
    }
}
