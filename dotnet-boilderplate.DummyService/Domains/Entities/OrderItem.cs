using dotnet_boilderplate.DummyService.Domains.ValueObjects;
using dotnet_boilderplate.SharedKernel.Common;

namespace dotnet_boilderplate.DummyService.Domains.Entities
{
    public class OrderItem : BaseEntity<EntityId>
    {
        public string ProductName { get; private set; }

        public int Quantity { get; private set; }

        public Money UnitPrice { get; private set; } = default!;

        public Money SubTotal => UnitPrice * Quantity;

        public static OrderItem Create(string productName, int quantity, Money unitPrice)
        => new()
        {
            Id = EntityId.New(),
            ProductName = productName,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
}
