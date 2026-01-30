using vrp_demo.Features.Commands.Drivers.AddSkillToDriver;
using vrp_demo.Features.Commands.Drivers.CreateDriver;
using vrp_demo.Features.Commands.Drivers.DeleteDriver;
using vrp_demo.Features.Commands.Drivers.UpdateDriver;
using vrp_demo.Features.Queries.Drivers.GetDriver;

namespace vrp_demo.Endpoints
{
    public static class DriversEndpoints
    {
        public static IEndpointRouteBuilder MapDriverEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapCreateDriver();
            builder.MapGetDriverEndpoint();
            builder.MapUpdateDriver();
            builder.MapDeleteDriver();
            builder.MapAddSkillToDriver();

            return builder;
        }
    }
}
