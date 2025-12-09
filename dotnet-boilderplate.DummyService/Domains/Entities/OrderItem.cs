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

        public string OrderId { get; private set; } = default!;

        public static OrderItem Create(string productName, int quantity, Money unitPrice)
        {
            var item = new OrderItem()
            {
                Id = EntityId.New(),
                ProductName = productName,
                Quantity = quantity,
                UnitPrice = unitPrice
            };

            item.SetCreated();

            return item;
        }

        public void UpdateQuantity(int quantity, string? updatedBy = null)
        {
            Quantity = quantity;
            SetUpdated(updatedBy);
        }

        public void SetOrderId(string orderId)
        {
            OrderId = orderId;
        }
    }
}
