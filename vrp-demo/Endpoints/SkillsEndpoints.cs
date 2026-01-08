using vrp_demo.Features.Commands.Skills.CreateSkill;
using vrp_demo.Features.Queries.Skills.GetSkills;

namespace vrp_demo.Endpoints
{
    public static class SkillsEndpoints
    {
        public static IEndpointRouteBuilder MapSkillsEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapCreateSkillEndpoint();
            builder.MapGetSkillsEndpoint();

            return builder;
        }
    }
}
