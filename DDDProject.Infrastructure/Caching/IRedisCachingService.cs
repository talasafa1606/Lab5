﻿using System.Threading.Tasks;

namespace DDDProject.Infrastructure.Caching
{
    public interface IRedisCachingService
    {
        Task SetAsync<T>(string key, T value);
        Task<T?> GetAsync<T>(string key);
        Task RemoveAsync(string key);
    }
}