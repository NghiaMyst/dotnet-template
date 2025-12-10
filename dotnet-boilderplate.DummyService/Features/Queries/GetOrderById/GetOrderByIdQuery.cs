using dotnet_boilderplate.DummyService.Domains.Aggregates;

namespace dotnet_boilderplate.DummyService.Features.Queries.GetOrderById;

public record GetOrderByIdQuery(string orderId);

public record GetOrderByIdResponse(
        string OrderId,
        string CustomerId,
        decimal TotalAmount,
        string Currency,
        string Status,
        DateTime OrderDate,
        List<OrderItemDto> Items
    );

public record OrderItemDto(
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        string Currency
    );