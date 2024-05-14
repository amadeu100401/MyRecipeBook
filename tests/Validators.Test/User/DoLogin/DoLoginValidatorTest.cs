using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Login.DoLogin;
using MyRecipeBook.Exceptions;

namespace Validators.Test.User.DoLogin;

public class DoLoginValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new DoLoginValidator();

        var request = RequestLoginJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().Be(true);
    }

    [Fact]
    public void Error_Email_Empty() 
    {
        var validator = new DoLoginValidator();

        var request = RequestLoginJsonBuilder.Build();
        request.Email = string.Empty;   

        var result = validator.Validate(request);

        result.IsValid.Should().Be(false);
        result.Errors.Should().ContainSingle().And
            .Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Email_Or_Password_Invalid));
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new DoLoginValidator();

        var request = RequestLoginJsonBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().Be(false);
        result.Errors.Should().ContainSingle().And
            .Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Email_Or_Password_Invalid));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new DoLoginValidator();

        var request = RequestLoginJsonBuilder.Build();
        request.Email = "invalidEmail";

        var result = validator.Validate(request);

        result.IsValid.Should().Be(false);
        result.Errors.Should().ContainSingle().And
            .Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Email_Or_Password_Invalid));
    }

    [Fact]
    public void Error_Password_Invalid()
    {
        var validator = new DoLoginValidator();

        var request = RequestLoginJsonBuilder.Build();
        request.Password = "1234";

        var result = validator.Validate(request);

        result.IsValid.Should().Be(false);
        result.Errors.Should().ContainSingle().And
            .Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Email_Or_Password_Invalid));
    }
}
