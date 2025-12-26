using Microsoft.AspNetCore.Mvc;
using dotnet_boilderplate.ServiceDefaults.Extensions;

namespace dotnet_template.AuthService.Features.Commands.RegisterUser
{
    public static class RegisterUserEndpoint
    {
        public static IEndpointRouteBuilder MapRegisterUserEndpoint(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/register", HandleRegisterUser)
                .WithName("RegisterUser")
                .WithTags("Users")
                .Produces<RegisterUserResponse>(201)
                .ProducesValidationProblem(400)
                .ProducesProblem(400);

            return builder;
        }

        public static async Task<IResult> HandleRegisterUser(
            [FromBody] RegisterUserRequest request,
            RegisterUserValidator validator,
            RegisterUserHandler handler,
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
                success => Results.Created($"/users/{success.Email}", success),
                failure => failure.ToProblemDetails()
            );
        }
    }
}
