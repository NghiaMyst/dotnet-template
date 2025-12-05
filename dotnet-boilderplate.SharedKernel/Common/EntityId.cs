namespace dotnet_boilderplate.SharedKernel.Common
{
    public readonly record struct EntityId
    {
        private readonly Guid _Id;

        public EntityId()
        {
            _Id = Guid.NewGuid();
        }

        private EntityId(Guid id) => _Id = id;
        private EntityId(String id) => _Id = Guid.Parse(id);

        public static EntityId New() => new EntityId(Guid.NewGuid());
        public static EntityId From(string value) => new(Guid.Parse(value));
        public override string ToString() => _Id.ToString();
        public static implicit operator string(EntityId id) => id._Id.ToString();
    }
}
