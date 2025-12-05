using FluentValidation;

namespace dotnet_boilderplate.DummyService.Features.CreateDummy
{
    public record CreateDummyRequest(
            string Name,
            List<CreateDummyItem> Items
        );

    public record CreateDummyItem(
            string TypeId,
            double Price,
            int Quantity
        );

    public class CreateDummyValidation : AbstractValidator<CreateDummyRequest>
    {

    }

    public class CreateDummyEndpoint
    {
    }
}
