using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Desactivate;
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Exceptions;

namespace Validators.Test.User.DesactivateUser;

public class DesactivateUseValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = BuildRequest();

        var result = BuildValidator(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Empty_Password()
    {
        var request = BuildRequest();
        request.UserPassword = string.Empty;    

        var result = BuildValidator(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Password_Empty));
    }

    [Fact]
    public void Error_Invalid_Password_Length()
    {
        var request = BuildRequest(1);

        var result = BuildValidator(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Password_Length_Invalid));
    }

    [Fact]
    public void Error_Invalid_Characters()
    {
        var request = BuildRequest();
        request.UserPassword = "12345678910";

        var result = BuildValidator(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(ex => ex.ErrorMessage.Equals(ResourceErroMensage.Password_Character));
    }

    private RequestDesactivateUserJson BuildRequest(int? passwordLength = null)
    {
        var  newRequest = new RequestDesactivateUserJsonBuilder();

        if (passwordLength.HasValue)
            return newRequest.Build((int)passwordLength);
        else
            return newRequest.Build();      
    }

    private FluentValidation.Results.ValidationResult BuildValidator(RequestDesactivateUserJson request) => new DesactiveUserValidator().Validate(request);
}
