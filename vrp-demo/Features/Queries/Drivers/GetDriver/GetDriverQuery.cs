namespace vrp_demo.Features.Queries.Drivers.GetDriver
{
    public record GetDriverQuery(Guid DriverId);

    public record GetDriverResponse(
        Guid Id,
        string Name,
        string Address,
        double Latitude,
        double Longitude,
        string Role,
        List<Guid> SkillIds
    );
}
