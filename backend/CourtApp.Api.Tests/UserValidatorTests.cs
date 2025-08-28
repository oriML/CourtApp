using FluentValidation.TestHelper;
using CourtApp.Api.DTOs;
using CourtApp.Api.Validators;
using Xunit;

namespace CourtApp.Api.Tests;

public class UserValidatorTests
{
    private readonly UserValidator _validator;

    public UserValidatorTests()
    {
        _validator = new UserValidator();
    }

    [Fact]
    public void Should_have_error_when_Username_is_empty()
    {
        var model = new UserDto { Username = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Should_not_have_error_when_Username_is_specified()
    {
        var model = new UserDto { Username = "testuser" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Username);
    }
}