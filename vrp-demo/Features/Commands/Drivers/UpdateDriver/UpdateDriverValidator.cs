using FluentValidation;

namespace vrp_demo.Features.Commands.Drivers.UpdateDriver
{
    public class UpdateDriverValidator : AbstractValidator<UpdateDriverRequest>
    {
        public UpdateDriverValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required");

            RuleFor(x => x.Role)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Role is required and must not exceed 20 characters");

            RuleFor(x => x.Lat)
                .NotEmpty()
                .NotNull()
                .GreaterThan(-90)
                .LessThanOrEqualTo(90)
                .WithMessage("Latitude must be between -90 and 90");

            RuleFor(x => x.Lng)
                .NotEmpty()
                .NotNull()
                .GreaterThan(-180)
                .LessThanOrEqualTo(180)
                .WithMessage("Longitude must be between -180 and 180");
        }
    }
}
