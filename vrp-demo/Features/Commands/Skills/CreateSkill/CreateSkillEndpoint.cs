using Microsoft.AspNetCore.Mvc;
using dotnet_boilderplate.ServiceDefaults.Extensions;

namespace vrp_demo.Features.Commands.Skills.CreateSkill;

public static class CreateSkillEndpoint
{
    public static IEndpointRouteBuilder MapCreateSkillEndpoint(this IEndpointRouteBuilder builder)
    {
        builder
            .MapPost("/skills", HandleCreateSkill)
            .WithName("CreateSkill")
            .WithTags("Skills")
            .Produces<CreateSkillResponse>(201)
            .ProducesValidationProblem(400)
            .ProducesProblem(400);

        return builder;
    }

    public static async Task<IResult> HandleCreateSkill(
            [FromBody] CreateSkillRequest request,
            CreateSkillValidator validator,
            CreateSkillHandler handler,
            HttpContext context,
            CancellationToken ct
        )
    {
        var validationResult = await validator.ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await handler.Handle(request, ct);

        // 3. Return
        return result.Match(
            success => Results.Created($"/orders/{success.SkillId}", success),
            failure => failure.ToProblemDetails()
        );
    }
}