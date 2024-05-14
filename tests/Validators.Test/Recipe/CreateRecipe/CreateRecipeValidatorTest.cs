using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.CreateRecipe;
using MyRecipeBook.Communication.Enum;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Recipe.CreateRecipe;

public class CreateRecipeValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = new RequestRegisterRecipeJsonBuilder().Build();

        var result = new CreateRecipeValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Erro_Title_Empty()
    {
        var request = new RequestRegisterRecipeJsonBuilder().Build();
        request.Title = string.Empty;

        var result = new CreateRecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Title_Empty));
    }

    [Fact]
    public void Erro_Category_Invalid()
    {
        var request = new RequestRegisterRecipeJsonBuilder().Build();
        request.Category = (Category)10000;

        var result = new CreateRecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Category_Invalid));
    }

    [Fact]
    public void Erro_Ingredient_Empty()
    {
        var request = new RequestRegisterRecipeJsonBuilder().Build();
        request.Ingredients = null;

        var result = new CreateRecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Ingredients_Empty));
    }

    [Fact]
    public void Erro_Method_Empty()
    {
        var request = new RequestRegisterRecipeJsonBuilder().Build();
        request.MethodPrepar = string.Empty;

        var result = new CreateRecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.MethodPrepar_Empty));
    }

    [Fact]
    public void Erro_Time_Empty()
    {
        var request = new RequestRegisterRecipeJsonBuilder().Build();
        request.TimePreparationMinutes = 0;

        var result = new CreateRecipeValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Time_Zero));
    }
}
