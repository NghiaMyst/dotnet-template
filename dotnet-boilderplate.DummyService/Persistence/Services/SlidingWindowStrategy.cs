using dotnet_boilderplate.ServiceDefaults.Contracts;
using StackExchange.Redis;

namespace dotnet_boilderplate.DummyService.Persistence.Services
{
    public class SlidingWindowStrategy(IConnectionMultiplexer redis) : IRateLimitService
    {
        private readonly IDatabase _db = redis.GetDatabase();

        public async Task<bool> IsAllowedAsync(string key, int limit, TimeSpan period)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var windowStart = now - (long)period.TotalMilliseconds;

            var luaScript = @"
                redis.call('ZREMRANGEBYSCORE', KEYS[1], 0, ARGV[1])
                local count = redis.call('ZCARD', KEYS[1])
                if count < tonumber(ARGV[2]) then
                    redis.call('ZADD', KEYS[1], ARGV[3], ARGV[3])
                    redis.call('PEXPIRE', KEYS[1], ARGV[4])
                    return 1
                end
                return 0";

            var result = (long)await _db.ScriptEvaluateAsync(luaScript,
                [new RedisKey(key)],
                [windowStart, limit, now, (int)period.TotalMilliseconds]);

            return result == 1;
        }

        /// <summary>
        /// Found the appropriate time start for current value
        /// Remove the expired
        /// Check if limit count still valid
        /// </summary>
        /// <param name="key"></param>
        /// <param name="limit"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        private async Task<bool> IsAllowedWithoutLuaAsync(string key, int limit, TimeSpan period)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var windowStart = now - (long)period.TotalMilliseconds;

            await _db.SortedSetRemoveRangeByScoreAsync(key, 0, windowStart);

            var count = await _db.SortedSetLengthAsync(key);

            if (count < limit)
            {
                await _db.SortedSetAddAsync(key, now, now);
                await _db.KeyExpireAsync(key, period);
                return true;
            }

            return false;
        }
    }
}
