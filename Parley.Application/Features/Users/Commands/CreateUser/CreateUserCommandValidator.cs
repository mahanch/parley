using FluentValidation;

namespace Parley.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandValidator:AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(u => u.FirstName).NotEmpty().WithMessage("First name is required");
        RuleFor(u => u.LastName).NotEmpty().WithMessage("Last name is required");
        RuleFor(u => u.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(u => u.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(u => u.Email).EmailAddress().WithMessage("Email is invalid");
        RuleFor(u => u.Password).NotEmpty().WithMessage("Password is required");
    }
}