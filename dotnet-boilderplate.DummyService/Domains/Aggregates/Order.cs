using dotnet_boilderplate.DummyService.Domains.Entities;
using dotnet_boilderplate.DummyService.Domains.Events;
using dotnet_boilderplate.DummyService.Domains.ValueObjects;
using dotnet_boilderplate.SharedKernel.Common;
using dotnet_boilderplate.SharedKernel.Results;

namespace dotnet_boilderplate.DummyService.Domains.Aggregates
{
    public class Order : BaseEntity<OrderId>
    {
        public CustomerId CustomerId { get; private set; } = default!;
        public DateTime Date { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public Money TotalAmount { get; private set; } = Money.Zero;

        private Order() {}

        public static Result<Order> Create(CustomerId customerId, List<OrderItem> items)
        {
            if (items.Count == 0)
                return Result.Failure<Order>(Error.Validation("Order must have at least one item"));

            var order = new Order
            {
                Id = OrderId.New(),
                CustomerId = customerId,
                Date = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            order.TotalAmount = items.Aggregate(Money.Zero, (sum, item) => sum + item.SubTotal);
            order.SetCreated();

            order.AddDomainEvent(new OrderCreatedDomainEvent(order.Id, order.CustomerId, order.TotalAmount));

            return Result.Success(order);
        }

        public void Confirm()
        {
            Status = OrderStatus.Confirmed;
            AddDomainEvent(new OrderConfirmedDomainEvent(Id));
        }
    }

    public enum OrderStatus
    { 
        Pending, Confirmed, Cancelled
    }
}
