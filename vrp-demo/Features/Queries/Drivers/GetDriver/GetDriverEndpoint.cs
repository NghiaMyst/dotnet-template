using dotnet_boilderplate.ServiceDefaults.Extensions;

namespace vrp_demo.Features.Queries.Drivers.GetDriver
{
    public static class GetDriverEndpoint
    {
        public static IEndpointRouteBuilder MapGetDriverEndpoint(this IEndpointRouteBuilder builder)
        {
            builder
                .MapGet("/drivers/{driverId:guid}", HandleGetDriver)
                .WithName("GetDriver")
                .WithTags("Driver")
                .Produces<GetDriverResponse>(200)
                .ProducesValidationProblem(400)
                .ProducesProblem(404);

            return builder;
        }

        public static async Task<IResult> HandleGetDriver(
                Guid driverId,
                GetDriverValidator validator,
                GetDriverHandler handler,
                HttpContext context,
                CancellationToken ct
            )
        {
            var query = new GetDriverQuery(driverId);
            var validationResult = await validator.ValidateAsync(query, ct);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.Handle(driverId);

            return result.Match(
                success => Results.Ok(success),
                failure => failure.ToProblemDetails()
            );
        }
    }
}
