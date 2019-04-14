using Hk.RedisCache.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hk.RedisCache
{
    public static class HkRedisServiceCollectionExtensions
    {
        public static IServiceCollection AddHkRedisCache(this IServiceCollection services, HkRedisOptions setupAction)
        {
            Guard.Guard.ArgumentNotNull(nameof(setupAction.ConnectionString), setupAction.ConnectionString);
            services.Configure<HkRedisOptions>(x =>
            {
                x.ConnectionString = setupAction.ConnectionString;
                x.DatabaseId = setupAction.DatabaseId;
                x.Timeout = setupAction.Timeout != default ? setupAction.Timeout : TimeSpan.FromHours(1);
            });
            //Redis connection must be singleton according to stackexchange docs!
            services.AddSingleton<IHkRedisCacheDatabaseProvider, HkRedisCacheDatabaseProvider>();
            services.AddTransient<ICacheManager, HkRedisCache>();
            services.AddTransient<ISerializer, Serializer>();
            services.AddTransient<CacheAttribute>();

            return services;
        }
    }
}
