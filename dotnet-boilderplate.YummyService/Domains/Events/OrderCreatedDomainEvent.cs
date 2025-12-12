using dotnet_boilderplate.SharedKernel.Common;
using dotnet_boilderplate.YummyService.Domains.ValueObjects;

namespace dotnet_boilderplate.YummyService.Domains.Events
{
    public record OrderCreatedDomainEvent(OrderId OrderId, CustomerId CustomerId, Money TotalAmount, DateTime CreatedAt)
        : DomainEventBase;
}
