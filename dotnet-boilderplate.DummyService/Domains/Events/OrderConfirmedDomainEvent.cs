using dotnet_boilderplate.DummyService.Domains.ValueObjects;
using dotnet_boilderplate.SharedKernel.Common;

namespace dotnet_boilderplate.DummyService.Domains.Events
{
    public record OrderConfirmedDomainEvent(OrderId OrderId)
        : DomainEventBase;
}
