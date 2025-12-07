using dotnet_boilderplate.ServiceDefaults.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_boilderplate.DummyService.Features.Commands.CreateOrder;

public static class CreateOrderEndpoint
{
    public static IEndpointRouteBuilder MapCreateOrderEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/orders", HandleCreateOrder)
            .WithName("CreateOrder")
            .WithTags("Orders")
            .Produces<CreateOrderResponse>(201)
            .ProducesValidationProblem(400)
            .ProducesProblem(400);

        return builder;
    }

    private static async Task<IResult> HandleCreateOrder(
        [FromBody] CreateOrderRequest request,
        CreateOrderValidator validator,
        CreateOrderHandler handler,
        HttpContext context,
        CancellationToken ct)
    {
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
            success => Results.Created($"/orders/{success.OrderId}", success),
            failure => failure.ToProblemDetails()
        );
    }
}

