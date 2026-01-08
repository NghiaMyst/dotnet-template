using dotnet_boilderplate.SharedKernel.Results;
using vrp_demo.Domains.ValueObjects;

namespace vrp_demo.Features.Queries.Skills.GetSkills
{
    public record GetSkillsQuery(int? PageSize, int? PageNumber);

    public record GetSkillsResponse(ResponseList<Skill> Skills);
}
