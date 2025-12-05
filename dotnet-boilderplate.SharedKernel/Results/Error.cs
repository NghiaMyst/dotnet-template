namespace dotnet_boilderplate.SharedKernel.Results
{
    public record Error(string Code, string Message)
    {
        public static readonly Error None = new(string.Empty, string.Empty);

        // Common errors can occur in variable services
        public static Error Failure(string message = "A failure occurred")
            => new("General.Failure", message);

        public static Error NotFound(string message = "Resource not found")
            => new("General.NotFound", message);

        public static Error Validation(string message = "Validation error")
            => new("General.Validation", message);

        public static Error Conflict(string message = "Conflict occurred")
            => new("General.Conflict", message);

        public static Error Unauthorized(string message = "Unauthorized access")
            => new("General.Unauthorized", message);

        public static Error Forbidden(string message = "Forbidden")
            => new("General.Forbidden", message);
    }
}
