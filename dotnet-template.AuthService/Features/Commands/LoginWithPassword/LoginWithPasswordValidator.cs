using FluentValidation;

namespace dotnet_template.AuthService.Features.Commands.LoginWithPassword
{
    public class LoginWithPasswordValidator : AbstractValidator<LoginWithPasswordRequest>
    {
        public LoginWithPasswordValidator()
        {
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email should not be empty and in the right format");

            RuleFor(e => e.Password)
                .NotEmpty();
        }
    }
}
