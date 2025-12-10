using dotnet_boilderplate.ServiceDefaults.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_boilderplate.DummyService.Features.Queries.GetOrderById
{
    public static class GetOrderByIdEndpoint
    {
        public static IEndpointRouteBuilder MapGetOrderByIdEndpoint(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/orders/{orderId}", HandleGetOrderById)
                .WithName("GetOrderById")
                .WithTags("Orders")
                .Produces<GetOrderByIdResponse>(201)
                .ProducesValidationProblem(400)
                .ProducesProblem(400);

            return builder;
        }


        private static async Task<IResult> HandleGetOrderById(
            [FromRoute] string orderId,
            GetOrderByIdValidator validator,
            GetOrderByIdHandler handler,
            HttpContext context,
            CancellationToken ct)
        {
            var request = new GetOrderByIdQuery(orderId);

            // 1. Validate
            var validationResult = await validator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            // 2. Handle
            var result = await handler.Handle(request, ct);

            // 3. Return
            return result.Match(
                success => Results.Ok(success),
                failure => failure.ToProblemDetails()
            );
        }
    }
}
