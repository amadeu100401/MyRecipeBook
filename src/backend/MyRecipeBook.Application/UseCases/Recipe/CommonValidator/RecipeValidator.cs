using FluentValidation;
using MyRecipeBook.Communication.Requests.RecipeRequest;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe.CommonValidator;

public class RecipeValidator : AbstractValidator<RequestRegisterRecipe> 
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Title).NotEmpty().WithMessage(ResourceErroMensage.Title_Empty);
        RuleFor(recipe => recipe.Category).IsInEnum().WithMessage(ResourceErroMensage.Category_Invalid);
        RuleFor(recipe => recipe.MethodPrepar).NotEmpty().WithMessage(ResourceErroMensage.MethodPrepar_Empty);
        RuleFor(recipe => recipe.Ingredients).NotEmpty().WithMessage(ResourceErroMensage.Ingredients_Empty);
        RuleFor(recipe => recipe.TimePreparationMinutes).GreaterThan(0).WithMessage(ResourceErroMensage.Time_Zero);
        RuleForEach(recipe => recipe.Ingredients).ChildRules(Ingredient =>
        { //Pra cada elemento da lista de ingredientes, será validado uma tributo diferente
            Ingredient.RuleFor(ingredient => ingredient.Name).NotEmpty().WithMessage(ResourceErroMensage.Ingredient_Name_Empty);
            Ingredient.RuleFor(ingredient => ingredient.Quantity).NotEmpty().WithMessage(ResourceErroMensage.Ingredient_Quantity_Empty);
            Ingredient.RuleFor(ingredient => ingredient.UnitMeasure).IsInEnum().WithMessage(ResourceErroMensage.Ingredient_UnitMeasure_Invalid);
        });
    }
}
