using dotnet_boilderplate.DummyService.Domains.Entities;
using dotnet_boilderplate.SharedKernel.Common;

namespace dotnet_boilderplate.DummyService.Features.Commands.CreateOrder
{
    public record CreateOrderRequest(string CustomerId, List<CreateOrderItemDto> Items)
    {
    }
    public record CreateOrderItemDto(string ProductName, int Quantity, decimal UnitPrice, string Currency = "VND");
    public record CreateOrderResponse(EntityId OrderId);
}
