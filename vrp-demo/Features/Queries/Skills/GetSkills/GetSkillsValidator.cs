using FluentValidation;

namespace vrp_demo.Features.Queries.Skills.GetSkills
{
    public class GetSkillsValidator : AbstractValidator<GetSkillsQuery>
    {
        public GetSkillsValidator()
        {
            RuleFor(q => q.PageNumber);

            RuleFor(q => q.PageSize);
        }
    }
}
