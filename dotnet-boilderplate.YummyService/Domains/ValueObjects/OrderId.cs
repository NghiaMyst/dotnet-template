using dotnet_boilderplate.SharedKernel.Common;

namespace dotnet_boilderplate.YummyService.Domains.ValueObjects
{
    public sealed class OrderId : ValueObject
    {
        public EntityId Value { get; }

        private OrderId(EntityId value) => Value = value;

        public static OrderId New() => new(EntityId.New());
        public static OrderId From(string value) => new(EntityId.From(value));
        public static OrderId From(EntityId value) => new(value);

        public override string ToString() => Value.ToString();
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
