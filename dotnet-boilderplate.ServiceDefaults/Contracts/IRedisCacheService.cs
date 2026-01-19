namespace dotnet_boilderplate.ServiceDefaults.Contracts
{
    public interface IRedisCacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task<bool> RemoveAsync(string key);

        #region Hash Set
        Task HashSetAsync<T>(string key, T obj, TimeSpan? expiration = null) where T : class;
        Task<T> HashGetAsync<T>(string key) where T : class, new();
        Task HashUpdateFieldAsync(string key, string fieldName, object value);
        #endregion

    }
}
