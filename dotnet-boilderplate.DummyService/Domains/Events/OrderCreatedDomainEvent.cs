using dotnet_boilderplate.DummyService.Domains.ValueObjects;
using dotnet_boilderplate.SharedKernel.Common;

namespace dotnet_boilderplate.DummyService.Domains.Events
{
    // When increasing in services, it can be modified into SharedKernel.Contracts
    public record OrderCreatedDomainEvent(OrderId OrderId, CustomerId CustomerId, Money TotalAmount, DateTime CreatedAt)
        : DomainEventBase;
}
