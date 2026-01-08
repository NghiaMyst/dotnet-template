using dotnet_boilderplate.ServiceDefaults.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace vrp_demo.Features.Commands.Drivers.CreateDriver
{
    public static class CreateDriverEndpoint
    {
        public static IEndpointRouteBuilder MapCreateDriver(this IEndpointRouteBuilder builder) 
        {
            builder
                .MapPost("/drivers", HandleCreateDriver)
                .WithName("CreateDriver")
                .WithTags("Driver")
                .Produces<CreateDriverResponse>(201)
                .ProducesValidationProblem(400)
                .ProducesProblem(400);

            return builder;
        }

        public static async Task<IResult> HandleCreateDriver(
                [FromBody] CreateDriverRequest request,
                CreateDriverValidator validator,
                CreateDriverHandler handler,
                HttpContext context,
                CancellationToken ct
            )
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.Handle(request.Name, request.Address, request.Lat, request.Lng, request.Role, request.SkillIds);

            return result.Match(
                    success => Results.Created($"/drivers/{success.DriverId}", success),
                    failure => failure.ToProblemDetails()
                );
        }
    }
}
