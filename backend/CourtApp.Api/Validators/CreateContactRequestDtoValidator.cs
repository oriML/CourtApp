using CourtApp.Api.DTOs;
using FluentValidation;

namespace CourtApp.Api.Validators;

public class CreateContactRequestDtoValidator : AbstractValidator<CreateContactRequestDto>
{
    public CreateContactRequestDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Phone).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Departments).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}