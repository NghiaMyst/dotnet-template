using dotnet_boilderplate.DummyService.Domains.ValueObjects;
using dotnet_boilderplate.SharedKernel.Common;

namespace dotnet_boilderplate.DummyService.Domains.Entities
{
    public class OutboxMessage : BaseEntity<OutboxMessageId>
    {
        public string Type { get; private set; } = string.Empty;
        /// <summary>
        /// JSON serialize
        /// </summary>
        public string Payload { get; private set; } = string.Empty;
        public bool Published { get; private set; } = false;

        private OutboxMessage() { }

        public OutboxMessage(IDomainEvent domainEvent)
        {
            Id = OutboxMessageId.New();
            Type = domainEvent.GetType().Name;
            Payload = Newtonsoft.Json.JsonConvert.SerializeObject(domainEvent);
            Published = false;
        }

        public void MarkAsPublished() => Published = true;
    }
}
