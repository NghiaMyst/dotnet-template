using dotnet_boilderplate.SharedKernel.Common;

namespace dotnet_boilderplate.DummyService.Domains.ValueObjects
{
    public sealed class Money : ValueObject
    {
        public decimal Amount { get; }

        public string Currency { get; }

        public static Money Zero => new(0m, "VND");

        public Money(decimal amount, string currency = "VND")
        {
            ArgumentOutOfRangeException.ThrowIfNegative(amount);

            Currency = string.IsNullOrWhiteSpace(currency) ? "VND" : currency.ToUpperInvariant();
            Amount = amount;
        }

        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw new InvalidOperationException("Currencies must be same");
            }

            return new Money(left.Amount + right.Amount, right.Currency);
        }

        public static Money operator *(Money money, int quantity)
        => new(money.Amount * quantity, money.Currency);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        public override string ToString() => $"{Amount:F2} {Currency}";
    }
}
