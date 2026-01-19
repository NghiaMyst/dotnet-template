using dotnet_boilderplate.ServiceDefaults.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace vrp_demo.Features.Commands.Jobs.CreateJob
{
    public static class CreateJobEndpoint
    {
        public static IEndpointRouteBuilder MapCreateJob(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/jobs", HandleCreateJob)
                .WithName("CreateJob")
                .WithTags("Job-Tasks")
                .Produces<CreateJobResponse>(201)
                .ProducesValidationProblem(400)
                .ProducesProblem(400);

            return builder;
        }

        public static async Task<IResult> HandleCreateJob(
            [FromBody] CreateJobRequest request,
            CreateJobValidator validator,
            CreateJobHandler handler,
            HttpContext context,
            CancellationToken ct)
        {
            var validationResult = await validator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.Handle(request.Description, request.JobType, request.Tasks, ct);

            return result.Match(
                    success => Results.Created($"/jobs/{success.Job.Id}", success),
                    failure => failure.ToProblemDetails()
                );
        }
    }
}
