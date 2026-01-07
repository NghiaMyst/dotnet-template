using FluentValidation;

namespace vrp_demo.Features.Commands.Skills.CreateSkill
{
    public class CreateSkillValidator : AbstractValidator<CreateSkillRequest>
    {
        public CreateSkillValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty();
        }
    }
}
