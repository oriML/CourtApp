using CourtApp.Api.DTOs;
using FluentValidation;

namespace CourtApp.Api.Validators;

public class UpdateContactRequestDtoValidator : AbstractValidator<UpdateContactRequestDto>
{
    public UpdateContactRequestDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Phone).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Departments).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}