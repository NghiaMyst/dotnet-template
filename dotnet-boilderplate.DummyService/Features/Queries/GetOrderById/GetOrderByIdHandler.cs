using dotnet_boilderplate.DummyService.Domains.ValueObjects;
using dotnet_boilderplate.DummyService.Persistence;
using dotnet_boilderplate.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace dotnet_boilderplate.DummyService.Features.Queries.GetOrderById
{
    public class GetOrderByIdHandler
    {
        private readonly DummyDbContext _dbContext;
        private readonly ILogger<GetOrderByIdHandler> _logger;

        public GetOrderByIdHandler(DummyDbContext dbContext, ILogger<GetOrderByIdHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result<GetOrderByIdResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var orderId = OrderId.From(request.orderId);

            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

            if (order == null)
            {
                return Result.Failure<GetOrderByIdResponse>(Error.NotFound($"Order {request.orderId} not found!"));
            }

            var orderItems = await _dbContext.OrderItems.Where(ot => ot.OrderId == order.Id.ToString()).ToListAsync();

            var response = new GetOrderByIdResponse(
                    order.Id.ToString(),
                    order.CustomerId.ToString(),
                    order.TotalAmount.Amount,
                    order.TotalAmount.Currency,
                    order.Status.ToString(),
                    order.Date,
                    [.. orderItems.Select(x => new OrderItemDto(
                            x.ProductName,
                            x.Quantity,
                            x.UnitPrice.Amount,
                            x.UnitPrice.Currency
                        ))]
                );

            return Result<GetOrderByIdResponse>.Success(response);
        }
    }
}
