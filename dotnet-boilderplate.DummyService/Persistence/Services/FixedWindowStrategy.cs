using dotnet_boilderplate.ServiceDefaults.Contracts;
using StackExchange.Redis;

namespace dotnet_boilderplate.DummyService.Persistence.Services
{
    /// <summary>
    /// Fixed async approach
    /// </summary>
    /// <param name="redis"></param>
    public class FixedWindowStrategy(IConnectionMultiplexer redis) : IRateLimitService
    {
        private readonly IDatabase _db = redis.GetDatabase();

        public async Task<bool> IsAllowedAsync(string key, int limit, TimeSpan period)
        {
            var luaScript = @"
                local count = redis.call('INCR', KEYS[1])
                
                if (count == 1) then
                    redis.call('EXPIRE', KEYS[1], ARGV[1])
                end
                if count > tonumber(ARGV[2]) then
                    return 0
                end

                return 1;
            ";

            var result = (long)await _db.ScriptEvaluateAsync(luaScript,
                    [new RedisKey(key)],
                    [(int)period.TotalSeconds, limit]);

            return result == 1;
        }

        private async Task<bool> IsAllowedWithoutLuaAsync(string key, int limit, TimeSpan period)
        {
            var count = await _db.StringIncrementAsync(key);

            if (count == 1)
            {
                await _db.KeyExpireAsync(key, period);
            }

            return count <= limit;
        }
    }
}
