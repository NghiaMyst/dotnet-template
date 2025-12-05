namespace dotnet_boilderplate.SharedKernel.Common
{
    public abstract class BaseEntity<TId> where TId : struct
    {
        public TId Id { get; set; }
        private readonly List<IDomainEvent> _domainEvents = new();
        
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
            => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
