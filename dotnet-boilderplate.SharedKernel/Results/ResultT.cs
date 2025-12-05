namespace dotnet_boilderplate.SharedKernel.Results
{
    public record Result<TValue> : Result
    {
        private readonly TValue? _value;
        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access Value when result is failure");

        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
            => _value = value;
    }
}
