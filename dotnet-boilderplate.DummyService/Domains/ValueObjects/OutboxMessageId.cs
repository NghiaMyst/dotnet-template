using dotnet_boilderplate.SharedKernel.Common;

namespace dotnet_boilderplate.DummyService.Domains.ValueObjects
{
    public sealed class OutboxMessageId : ValueObject
    {
        public EntityId Value { get; }

        private OutboxMessageId(EntityId value) => Value = value;

        public static OutboxMessageId New() => new(EntityId.New());
        public static OutboxMessageId From(string value) => new(EntityId.From(value));
        public static OutboxMessageId From(EntityId value) => new(value);

        public override string ToString() => Value.ToString();

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
