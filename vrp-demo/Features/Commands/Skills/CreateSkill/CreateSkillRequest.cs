namespace vrp_demo.Features.Commands.Skills.CreateSkill
{
    public record CreateSkillRequest(string Name, string? Description);

    public record CreateSkillResponse(Guid skillId);
}
