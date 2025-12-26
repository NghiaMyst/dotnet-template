using FluentValidation;

namespace dotnet_template.AuthService.Features.Commands.RegisterUser
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid email format");

            RuleFor(e => e.Password)
                .NotEmpty()
                .MinimumLength(6)
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit");

            RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match");
        }
    }
}
