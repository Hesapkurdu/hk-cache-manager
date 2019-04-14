using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hk.RedisCache
{
    public class RedisResponseCacheAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => true;

        public int Duration { get; set; }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetService<CacheAttribute>();
            if (Duration > default(int))
            {
                filter.Duration = Duration;
            }
            return filter;
        }
    }
}
