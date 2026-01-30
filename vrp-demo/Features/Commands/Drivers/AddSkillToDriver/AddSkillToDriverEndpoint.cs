using dotnet_boilderplate.ServiceDefaults.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace vrp_demo.Features.Commands.Drivers.AddSkillToDriver
{
    public static class AddSkillToDriverEndpoint
    {
        public static IEndpointRouteBuilder MapAddSkillToDriver(this IEndpointRouteBuilder builder)
        {
            builder
                .MapPost("/drivers/{driverId:guid}/skills", HandleAddSkillToDriver)
                .WithName("AddSkillToDriver")
                .WithTags("Driver")
                .Produces<AddSkillToDriverResponse>(200)
                .ProducesValidationProblem(400)
                .ProducesProblem(404);

            return builder;
        }

        public static async Task<IResult> HandleAddSkillToDriver(
                Guid driverId,
                [FromBody] AddSkillToDriverRequest request,
                AddSkillToDriverValidator validator,
                AddSkillToDriverHandler handler,
                HttpContext context,
                CancellationToken ct
            )
        {
            var validationResult = await validator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.Handle(driverId, request.SkillIds);

            return result.Match(
                    success => Results.Ok(success),
                    failure => failure.ToProblemDetails()
                );
        }
    }
}
