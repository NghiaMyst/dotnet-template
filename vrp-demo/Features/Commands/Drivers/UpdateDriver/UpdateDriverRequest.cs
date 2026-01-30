namespace vrp_demo.Features.Commands.Drivers.UpdateDriver
{
    public record UpdateDriverRequest(string Name, string Address, double Lat, double Lng, string Role);

    public record UpdateDriverResponse(Guid DriverId, string Name, string Address);
}
