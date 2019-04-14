using StackExchange.Redis;
using System;

namespace Hk.RedisCache.Contracts
{
    public interface IHkRedisCacheDatabaseProvider
    {
        TimeSpan Timeout { get; set; }
        IDatabase GetDatabase();
    }
}
