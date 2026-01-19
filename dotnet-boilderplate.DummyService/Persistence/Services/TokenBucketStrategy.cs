using dotnet_boilderplate.ServiceDefaults.Contracts;
using StackExchange.Redis;

namespace dotnet_boilderplate.DummyService.Persistence.Services
{
    public class TokenBucketStrategy(IConnectionMultiplexer redis) : IRateLimitService
    {
        private readonly IDatabase _db = redis.GetDatabase();

        public async Task<bool> IsAllowedAsync(string key, int limit, TimeSpan period)
        {
            return true;
        }
    }
}
