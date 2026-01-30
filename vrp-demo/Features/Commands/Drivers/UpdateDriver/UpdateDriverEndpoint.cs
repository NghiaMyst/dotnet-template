using dotnet_boilderplate.ServiceDefaults.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace vrp_demo.Features.Commands.Drivers.UpdateDriver
{
    public static class UpdateDriverEndpoint
    {
        public static IEndpointRouteBuilder MapUpdateDriver(this IEndpointRouteBuilder builder)
        {
            builder
                .MapPut("/drivers/{driverId:guid}", HandleUpdateDriver)
                .WithName("UpdateDriver")
                .WithTags("Driver")
                .Produces<UpdateDriverResponse>(200)
                .ProducesValidationProblem(400)
                .ProducesProblem(404);

            return builder;
        }

        public static async Task<IResult> HandleUpdateDriver(
                Guid driverId,
                [FromBody] UpdateDriverRequest request,
                UpdateDriverValidator validator,
                UpdateDriverHandler handler,
                HttpContext context,
                CancellationToken ct
            )
        {
            var validationResult = await validator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.Handle(driverId, request.Name, request.Address, request.Lat, request.Lng, request.Role);

            return result.Match(
                    success => Results.Ok(success),
                    failure => failure.ToProblemDetails()
                );
        }
    }
}
