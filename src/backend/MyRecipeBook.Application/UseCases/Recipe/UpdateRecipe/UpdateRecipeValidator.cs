using FluentValidation;
using MyRecipeBook.Application.UseCases.Recipe.CommonValidator;
using MyRecipeBook.Communication.Requests.RecipeRequest;

namespace MyRecipeBook.Application.UseCases.Recipe.UpdateRecipe;

public class UpdateRecipeValidator : AbstractValidator<RequestRegisterRecipe>
{
    public UpdateRecipeValidator()
    {
        RuleFor(recipe => recipe).SetValidator(new RecipeValidator());
    }
}
