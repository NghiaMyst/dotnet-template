using dotnet_boilderplate.ServiceDefaults.Extensions;
using dotnet_template.AuthService.Features.Commands.RegisterUser;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_template.AuthService.Features.Commands.LoginWithPassword
{
    public static class LoginWithPasswordEndpoint
    {
        public static IEndpointRouteBuilder MapLoginWithPassword(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/login", HandleLoginWithPassword)
                .WithName("LoginWithPassword")
                .WithTags("Users")
                .Produces<LoginWithPasswordResponse>(201)
                .ProducesValidationProblem(400)
                .ProducesProblem(400);

            return builder;
        }

        public static async Task<IResult> HandleLoginWithPassword(
            [FromBody] LoginWithPasswordRequest request,
            LoginWithPasswordValidator validator,
            LoginWithPasswordHandler handler,
            HttpContext context,
            CancellationToken ct)
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.Handle(request, ct);

            return result.Match(
                success => Results.Ok(success.Token),
                failure => failure.ToProblemDetails()
            );
        }
    }
}
