using FluentValidation;

namespace dotnet_boilderplate.DummyService.Features.Queries.GetOrderById
{
    public class GetOrderByIdValidator : AbstractValidator<GetOrderByIdQuery>
    {
        public GetOrderByIdValidator() 
        {
            RuleFor(x => x.orderId).NotEmpty().WithMessage("OrderId required");
        }
    }
}
