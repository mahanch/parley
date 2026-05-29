using FluentValidation;

namespace Parley.Application.Features.Users.Commands.CreateUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.Username).NotEmpty().WithMessage("نام کاربری الزامی است.");
        RuleFor(u => u.Email).NotEmpty().WithMessage("ایمیل الزامی است.")
            .EmailAddress().WithMessage("فرمت ایمیل نامعتبر است.");
        RuleFor(u => u.Password).NotEmpty().WithMessage("رمز عبور الزامی است.")
            .MinimumLength(6).WithMessage("رمز عبور باید حداقل ۶ کاراکتر باشد.");
    }
}