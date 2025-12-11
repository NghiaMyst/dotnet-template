namespace dotnet_boilderplate.SharedKernel.Messaging
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync(string routingKeyOrTopic, string payload, CancellationToken cancellationToken);
    }
}
