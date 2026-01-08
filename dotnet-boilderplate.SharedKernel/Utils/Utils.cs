namespace dotnet_boilderplate.SharedKernel.Utils
{
    public static class Utils
    {
        public static string GetLoggerPrefix<T>() where T : class
        {
            return $"[{nameof(T)}]";
        }
    }
}
