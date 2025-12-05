namespace dotnet_boilderplate.SharedKernel.Common
{
    public interface INotification
    {

    }

    public interface IDomainEvent : INotification
    {
        Guid EventId => Guid.NewGuid();
        DateTime OccurredOn => DateTime.UtcNow;
    }

    public abstract record DomainEventBase : IDomainEvent;
}
