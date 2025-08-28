using FluentValidation.TestHelper;
using CourtApp.Api.DTOs;
using CourtApp.Api.Validators;
using Xunit;

namespace CourtApp.Api.Tests;

public class CreateContactRequestDtoValidatorTests
{
    private readonly CreateContactRequestDtoValidator _validator;

    public CreateContactRequestDtoValidatorTests()
    {
        _validator = new CreateContactRequestDtoValidator();
    }

    [Fact]
    public void Should_have_error_when_Name_is_empty()
    {
        var model = new CreateContactRequestDto { Name = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_not_have_error_when_Name_is_specified()
    {
        var model = new CreateContactRequestDto { Name = "John Doe" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_have_error_when_Email_is_invalid()
    {
        var model = new CreateContactRequestDto { Email = "invalid-email" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_not_have_error_when_Email_is_valid()
    {
        var model = new CreateContactRequestDto { Email = "test@example.com" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_have_error_when_Phone_is_empty()
    {
        var model = new CreateContactRequestDto { Phone = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Should_not_have_error_when_Phone_is_specified()
    {
        var model = new CreateContactRequestDto { Phone = "1234567890" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Should_have_error_when_Description_is_empty()
    {
        var model = new CreateContactRequestDto { Description = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_not_have_error_when_Description_is_specified()
    {
        var model = new CreateContactRequestDto { Description = "Some description" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_have_error_when_Departments_is_empty()
    {
        var model = new CreateContactRequestDto { Departments = System.Array.Empty<string>() };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Departments);
    }

    [Fact]
    public void Should_not_have_error_when_Departments_is_specified()
    {
        var model = new CreateContactRequestDto { Departments = new[] { "Department1" } };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Departments);
    }
}
