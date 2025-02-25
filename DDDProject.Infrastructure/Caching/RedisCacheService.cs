using System;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace DDDProject.Infrastructure.Caching
{
    public class RedisCacheService : IRedisCachingService
    {
        private readonly IDatabase _cacheDb;
        private readonly TimeSpan _cacheLifetime = TimeSpan.FromMinutes(30);

        public RedisCacheService()
        {
            var redis = ConnectionMultiplexer.Connect("host.docker.internal:6379,abortConnect=false");
            _cacheDb = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value)
        {
            var jsonData = JsonSerializer.Serialize(value);
            await _cacheDb.StringSetAsync(key, jsonData, _cacheLifetime);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var data = await _cacheDb.StringGetAsync(key);
            return data.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(data);
        }

        public async Task RemoveAsync(string key)
        {
            await _cacheDb.KeyDeleteAsync(key);
        }
    }
}