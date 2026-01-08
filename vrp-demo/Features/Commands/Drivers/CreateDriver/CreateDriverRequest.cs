using vrp_demo.Domains.Aggregates;

namespace vrp_demo.Features.Commands.Drivers.CreateDriver
{
    public record CreateDriverRequest(string Name, string Address, double Lat, double Lng, string Role, List<Guid>? SkillIds = null);

    public record CreateDriverResponse(Guid DriverId);
}
