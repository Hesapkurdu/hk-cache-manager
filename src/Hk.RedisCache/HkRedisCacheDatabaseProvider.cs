using Hk.RedisCache.Contracts;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;

namespace Hk.RedisCache
{
    public class HkRedisCacheDatabaseProvider : IHkRedisCacheDatabaseProvider
    {
        private readonly HkRedisOptions _options;
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        public HkRedisCacheDatabaseProvider(IOptions<HkRedisOptions> options)
        {
            _options = options.Value;
            Timeout = options.Value.Timeout;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }
        public TimeSpan Timeout { get; set; }

        /// <summary>
        ///     Gets the database connection.
        /// </summary>
        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase(_options.DatabaseId);
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_options.ConnectionString);
        }
    }
}
