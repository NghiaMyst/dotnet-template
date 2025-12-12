using dotnet_boilderplate.SharedKernel.Common;

namespace dotnet_boilderplate.YummyService.Domains.ValueObjects
{
    public sealed class CustomerId : ValueObject
    {
        public EntityId Value { get; }

        private CustomerId(EntityId value) => Value = value;
        public static CustomerId From(string value) => new(EntityId.From(value));
        public static CustomerId From(EntityId value) => new(value);

        public override string ToString() => Value.ToString();

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
