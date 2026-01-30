namespace vrp_demo.Features.Commands.Drivers.AddSkillToDriver
{
    public record AddSkillToDriverRequest(List<Guid> SkillIds);

    public record AddSkillToDriverResponse(Guid DriverId, List<Guid> SkillIds);
}
