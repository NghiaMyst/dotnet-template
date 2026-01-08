using dotnet_boilderplate.ServiceDefaults.Extensions;

namespace vrp_demo.Features.Queries.Skills.GetSkills
{
    public static class GetSkillsEndpoint
    {
        public static IEndpointRouteBuilder MapGetSkillsEndpoint(this IEndpointRouteBuilder builder)
        {
            builder
                .MapGet("/skills", HandleGetSkills)
                .WithName("GetSkillsWithPaging")
                .WithTags("Skills")
                .Produces<GetSkillsResponse>(201)
                .ProducesValidationProblem(400)
                .ProducesProblem(400);

            return builder;
        }

        public static async Task<IResult> HandleGetSkills(
                [AsParameters] GetSkillsQuery query,
                GetSkillsValidator validator,
                GetSkillsHandler handler,
                HttpContext context, 
                CancellationToken ct
            )
        {
            var validationResult = await validator.ValidateAsync(query, ct);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.Handle(query.PageNumber, query.PageSize);

            return result.Match(
                success => Results.Ok(success.Data),
                failure => failure.ToProblemDetails()
            );
        }
    }
}
