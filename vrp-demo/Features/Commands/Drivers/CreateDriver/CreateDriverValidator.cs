using FluentValidation;

namespace vrp_demo.Features.Commands.Drivers.CreateDriver
{
    public class CreateDriverValidator : AbstractValidator<CreateDriverRequest>
    {
        public CreateDriverValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Address);

            RuleFor(x => x.Role)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Lat)
                .NotEmpty()
                .NotNull()
                .GreaterThan(-90)
                .LessThanOrEqualTo(90);

            RuleFor(x => x.Lng)
                .NotEmpty()
                .NotNull()
                .GreaterThan(-180)
                .LessThanOrEqualTo(180);
        }
    }
}
