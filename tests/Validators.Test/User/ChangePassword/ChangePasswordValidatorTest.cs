using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Exceptions;

namespace Validators.Test.User.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = new RequestChangePasswordJsonBuilder().Build();

        var result = new ChangePasswordValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Empty_Password()
    {
        var request = new RequestChangePasswordJsonBuilder().Build();
        request.NewPassword = string.Empty;

        var result = new ChangePasswordValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Password_Empty));
    }

    [Fact]
    public void Error_Invalid_Password_Lenght()
    {
        var request = new RequestChangePasswordJsonBuilder().Build();
        request.NewPassword = "123";

        var result = new ChangePasswordValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Password_Length_Invalid));
    }

    [Fact]
    public void Error_Invalid_Character_Lenght()
    {
        var request = new RequestChangePasswordJsonBuilder().Build();
        request.NewPassword = "teste1";

        var result = new ChangePasswordValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Password_Character));
    }
}
