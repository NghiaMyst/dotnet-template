using dotnet_boilderplate.SharedKernel.Common;

namespace dotnet_template.AuthService.Domains.ValueObjects
{
    public sealed class UserId : ValueObject
    {
        public EntityId Value { get; }

        private UserId() => Value = EntityId.New();

        private UserId(EntityId value) => Value = value;

        public static UserId New() => new(EntityId.New());
        public static UserId From(string value) => new(EntityId.From(value));
        public static UserId From(EntityId value) => new(value);

        public override string ToString() => Value.ToString();

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
