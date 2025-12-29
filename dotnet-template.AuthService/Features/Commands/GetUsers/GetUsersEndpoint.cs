using dotnet_boilderplate.ServiceDefaults.Extensions;
using dotnet_template.AuthService.Persistence.Authorization;

namespace dotnet_template.AuthService.Features.Commands.GetUsers
{
    public static class GetUsersEndpoint
    {
        public static IEndpointRouteBuilder MapGetUsers(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("users", GetUsers)
                .WithName("GetUsersWithPaging")
                .WithTags("Users")
                .Produces<GetUsersResponse>(201)
                .ProducesValidationProblem(400)
                .ProducesProblem(400)
                .RequireAuthorization(Policies.CanViewUsers);

            return builder;
        }

        public static async Task<IResult> GetUsers(
            [AsParameters] GetUsersRequest request,
            GetUsersValidator validator,
            GetUsersHandler handler,
            HttpContext context,
            CancellationToken ct)
        {
            var validationResult = await validator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.Handle(request, ct);

            return result.Match(
                success => Results.Ok(success.Users),
                failure => failure.ToProblemDetails()
            );
        }
    }
}
