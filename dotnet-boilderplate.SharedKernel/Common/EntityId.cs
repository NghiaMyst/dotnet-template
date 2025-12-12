
using dotnet_boilderplate.SharedKernel.Utils;
using Newtonsoft.Json;

namespace dotnet_boilderplate.SharedKernel.Common
{
    [JsonConverter(typeof(EntityIdJsonConverter))]
    public sealed class EntityId : ValueObject
    {
        private readonly Guid _Id;

        public EntityId()
        {
            _Id = Guid.NewGuid();
        }

        private EntityId(Guid id) => _Id = id;
        private EntityId(String id) => _Id = Guid.Parse(id);

        public static EntityId New() => new(Guid.NewGuid());
        public static EntityId From(string value) => new(value);
        public override string ToString() => _Id.ToString();


        public static implicit operator string(EntityId id) => id._Id.ToString();
        protected override IEnumerable<object?> GetEqualityComponents()
        {
           yield return _Id;
        }
    }

    // EntityId acts as a data holder, based on strong typedId pattern
}
