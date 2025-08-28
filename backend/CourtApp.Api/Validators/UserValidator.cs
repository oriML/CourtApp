using CourtApp.Api.DTOs;
using FluentValidation;

namespace CourtApp.Api.Validators;

public class UserValidator : AbstractValidator<UserDto>
{
    public UserValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
    }
}