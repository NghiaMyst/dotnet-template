using dotnet_boilderplate.ServiceDefaults.Contracts;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace vrp_demo.Persistence.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _db;

        private readonly IConnectionMultiplexer _redis;

        private ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
            _logger = logger;
        }

        #region Basic Functions
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);

            if (String.IsNullOrEmpty(value)) return default;

            return JsonConvert.DeserializeObject<T>(value!);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                return await _db.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("[RedisCache]: {ex}", ex.Message);
                return false;
            }
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var stringValue = JsonConvert.SerializeObject(value);

            try
            {
                return await _db.StringSetAsync(key, stringValue);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("[RedisCache]: {ex}", ex.Message);
                return false;
            }
        }
        #endregion

        #region Hash Set Object
        public async Task<T> HashGetAsync<T>(string key) where T : class, new()
        {
            var entries = await _db.HashGetAllAsync(key);

            if (entries.Length == 0) return null;

            var obj = new T();
            var properties = typeof(T).GetProperties();

            foreach (var entry in entries)
            {
                var property = properties.FirstOrDefault(p => p.Name == entry.Name);
                if (property != null && entry.Value.HasValue)
                {
                    var val = Convert.ChangeType(entry.Value, property.PropertyType);
                    property.SetValue(obj, val);
                }
            }

            return obj;
        }

        public async Task HashSetAsync<T>(string key, T obj, TimeSpan? expiration = null) where T : class
        {
            var properties = typeof(T).GetProperties();

            var entries = properties.Select(p => new HashEntry(p.Name, p.GetValue(obj)?.ToString() ?? "")).ToArray();

            await _db.HashSetAsync(key, entries);

            if (expiration.HasValue)
            {
                await _db.KeyExpireAsync(key, expiration);
            }
        }

        public async Task HashUpdateFieldAsync(string key, string fieldName, object value)
        {
            await _db.HashSetAsync(key, fieldName, value?.ToString());
        }
        #endregion

        #region List entities with ids
        public async Task HashSetListAsync<T>(string key, List<T> entities, TimeSpan? expiration = null) where T : class
        {
            if (entities == null || !entities.Any()) return;

            var idProperty = typeof(T).GetProperty("id") ?? typeof(T).GetProperty("Id");

            if (idProperty == null) return;

            var entries = entities.Select(entity =>
            {
                var idValue = idProperty.GetValue(entity)?.ToString() ?? Guid.NewGuid().ToString();
                var jsonValue = JsonConvert.SerializeObject(entity);
                return new HashEntry(idValue, jsonValue);
            }).ToArray();

            await _db.HashSetAsync(key, entries);

            if (expiration.HasValue)
            {
                await _db.KeyExpireAsync(key, expiration);
            }
        }
        #endregion
    }
}
