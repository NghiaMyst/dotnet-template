using FluentValidation;

namespace vrp_demo.Features.Queries.Drivers.GetDriver
{
    public class GetDriverValidator : AbstractValidator<GetDriverQuery>
    {
        public GetDriverValidator()
        {
            RuleFor(x => x.DriverId)
                .NotEmpty()
                .WithMessage("Driver ID is required");
        }
    }
}
