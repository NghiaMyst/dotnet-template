using FluentValidation;

namespace dotnet_template.AuthService.Features.Commands.GetUsers
{
    public class GetUsersValidator : AbstractValidator<GetUsersRequest>
    {
        public GetUsersValidator()
        {
            RuleFor(r => r.PageNumber);

            RuleFor(r => r.PageSize);
        }
    }
}
