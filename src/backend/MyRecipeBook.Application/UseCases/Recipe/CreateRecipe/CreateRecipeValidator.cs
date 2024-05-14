using FluentValidation;
using MyRecipeBook.Application.UseCases.Recipe.CommonValidator;
using MyRecipeBook.Communication.Requests.RecipeRequest;

namespace MyRecipeBook.Application.UseCases.Recipe.CreateRecipe;

public class CreateRecipeValidator : AbstractValidator<RequestRegisterRecipe>
{
    public CreateRecipeValidator()
    {
        RuleFor(recipe => recipe).SetValidator(new RecipeValidator());
    }
}
