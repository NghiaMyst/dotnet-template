using FluentValidation;

namespace vrp_demo.Features.Commands.Drivers.AddSkillToDriver
{
    public class AddSkillToDriverValidator : AbstractValidator<AddSkillToDriverRequest>
    {
        public AddSkillToDriverValidator()
        {
            RuleFor(x => x.SkillIds)
                .NotNull()
                .NotEmpty()
                .WithMessage("Skill IDs cannot be empty");

            RuleForEach(x => x.SkillIds)
                .NotEmpty()
                .WithMessage("Each skill ID must be valid");
        }
    }
}
