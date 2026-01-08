using vrp_demo.Features.Commands.Drivers.CreateDriver;

namespace vrp_demo.Endpoints
{
    public static class DriversEndpoints
    {
        public static IEndpointRouteBuilder MapDriverEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapCreateDriver();

            return builder;
        }
    }
}
