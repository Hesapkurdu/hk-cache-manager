using Hk.RedisCache.Contracts;
using JetBrains.Annotations;
using StackExchange.Redis;
using System;

namespace Hk.RedisCache
{
    public class HkRedisCache : ICacheManager
    {
        private readonly IDatabase _database;
        private readonly IHkRedisCacheDatabaseProvider _databaseProvider;
        private readonly ISerializer _serializer;

        public HkRedisCache(IHkRedisCacheDatabaseProvider databaseProvider, ISerializer serializer)
        {
            _databaseProvider = databaseProvider;
            _serializer = serializer;
            _database = _databaseProvider.GetDatabase();
        }

        public string GetCache(string name)
        {
            var response = _database.StringGet(name);
            return response;
        }

        public T GetCache<T>(string name)
        {
            var response = _database.StringGet(name);
            if (!response.HasValue)
                return default(T);
            return _serializer.Deserialize<T>(response);
        }

        public T GetCache<T>(T @object)
        {
            var name = RedisHelper.KeyGenerator(@object);
            var response = _database.StringGet(name);
            if (!response.HasValue)
                return default(T);
            return _serializer.Deserialize<T>(response);
        }

        public void SetCache(string name, string value)
        {
            SetCache(name, value, _databaseProvider.Timeout);
        }

        public void SetCache(string name, string value, TimeSpan timeSpan)
        {
            _database.StringSet(name, value, timeSpan);
        }

        public void SetCache<T>(string name, T value, TimeSpan timeout)
        {
            _database.StringSet(name, _serializer.Serialize(value), timeout);
        }

        public void SetCache<T>([NotNull] string name, T value)
        {
            SetCache(name, value, _databaseProvider.Timeout);
        }

        public void SetCache<T>(T @object)
        {
            var name = RedisHelper.KeyGenerator(@object);
            SetCache(name, @object);
        }

        public T GetFromCacheOrRun<T>(string cacheKey, Func<T> action)
        {
            var result = GetCache<T>(cacheKey);
            if (result is null)
            {
                var invokeResult = action.Invoke();
                SetCache(cacheKey, invokeResult);
                return invokeResult;
            }
            return result;
        }
    }
}
