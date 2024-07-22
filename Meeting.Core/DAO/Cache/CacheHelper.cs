using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Meeting.Core.DAO.Cache
{
    public class CacheHelper: ICacheHelper
    {
        private readonly IDistributedCache _cache;

        public CacheHelper(IDistributedCache cache) {  _cache = cache; }

        public async Task<T?> GetAsync<T>(string key, Func<Task<T?>> missFunc)
        {
            string? json = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(json))
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            T? target = await missFunc();
            if (target != null)
            {
                string value = JsonSerializer.Serialize<T>(target);
                await _cache.SetStringAsync(key, value);
            }
            return target;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            string? json = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(json))
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            return default;
        }

        public async Task SetAsync<T>(string key, T value)
        {
            string json = JsonSerializer.Serialize<T>(value);
            await _cache.SetStringAsync(key, json);
        }

        public async Task DelAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
