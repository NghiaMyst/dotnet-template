using dotnet_boilderplate.DummyService.Domains.Aggregates;
using dotnet_boilderplate.DummyService.Domains.Entities;
using dotnet_boilderplate.DummyService.Domains.ValueObjects;
using dotnet_boilderplate.DummyService.Persistence;
using dotnet_boilderplate.SharedKernel.Results;

namespace dotnet_boilderplate.DummyService.Features.Commands.CreateOrder
{
    public class CreateOrderHandler
    {
        private readonly DummyDbContext _dbContext;

        private readonly ILogger<CreateOrderHandler> _logger;

        public CreateOrderHandler(DummyDbContext dbContext, ILogger<CreateOrderHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result<CreateOrderResponse>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var customerId = CustomerId.From(request.CustomerId);

            var orderItems = request.Items
            .Select(item => OrderItem.Create(
                productName: item.ProductName,
                quantity: item.Quantity,
                unitPrice: new Money(item.UnitPrice, item.Currency)
            ))
            .ToList();

            // Domain logic
            var orderResult = Order.Create(customerId, orderItems);
            if (orderResult.IsFailure)
                return Result.Failure<CreateOrderResponse>(orderResult.Error);

            var order = orderResult.Value;

            // Persist
            //_dbContext.Orders.Add(order);
            //await _dbContext.SaveChangesAsync(ct);

            // Return
            return Result.Success(new CreateOrderResponse(order.Id.Value));
        }
    }
}
