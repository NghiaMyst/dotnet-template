namespace dotnet_boilderplate.ServiceDefaults.Contracts
{
    public interface IRateLimitService
    {
        Task<bool> IsAllowedAsync(string key, int limit, TimeSpan period);
    }
}
