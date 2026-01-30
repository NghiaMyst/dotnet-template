using dotnet_boilderplate.ServiceDefaults.Extensions;

namespace vrp_demo.Features.Commands.Drivers.DeleteDriver
{
    public static class DeleteDriverEndpoint
    {
        public static IEndpointRouteBuilder MapDeleteDriver(this IEndpointRouteBuilder builder)
        {
            builder
                .MapDelete("/drivers/{driverId:guid}", HandleDeleteDriver)
                .WithName("DeleteDriver")
                .WithTags("Driver")
                .Produces<DeleteDriverResponse>(200)
                .ProducesProblem(404);

            return builder;
        }

        public static async Task<IResult> HandleDeleteDriver(
                Guid driverId,
                DeleteDriverHandler handler,
                HttpContext context,
                CancellationToken ct
            )
        {
            var result = await handler.Handle(driverId);

            return result.Match(
                    success => Results.Ok(success),
                    failure => failure.ToProblemDetails()
                );
        }
    }
}
