namespace dotnet_boilderplate.SharedKernel.Common
{
    public abstract class BaseEntity<TId> : RootBaseEntity
    {
        public TId Id { get; set; }
    }
}
