using FluentValidation;
using FluentValidation.Validators;
using MyRecipeBook.Exceptions;
using System.Text.RegularExpressions;

namespace MyRecipeBook.Application.SheredValidator;

public class PasswordValidator<T> : PropertyValidator<T,string>
{
    public override bool IsValid(ValidationContext<T> context, string password)
    {
        // Regex para verificar se a senha contém pelo menos um número e uma letra maiúscula
        Regex regex = new Regex(@"^(?=.*\d)(?=.*[A-Z]).*$");

        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceErroMensage.Password_Empty);

            return false;
        }

        if (password.Length < 6)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceErroMensage.Password_Length_Invalid);

            return false;
        }

        if (!regex.IsMatch(password))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceErroMensage.Password_Character);

            return false;
        }

        return true;
    }

    public override string Name => "PasswordValidator";

    protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}"; 
}
