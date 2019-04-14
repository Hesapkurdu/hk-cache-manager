using Hk.RedisCache.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hk.RedisCache
{
    internal sealed class CacheAttribute : ResultFilterAttribute, IActionFilter
    {
        /// <summary>
        ///     Duration in seconds!. TimeSpan cannot be used as Attribute parameters on current .Net
        /// </summary>
        public int Duration { set; get; } = (int)TimeSpan.FromHours(1).TotalSeconds;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var cacheManager = context.HttpContext.RequestServices.GetService<ICacheManager>();
            var key = FindRedisCacheKey(context.HttpContext);
            if (context.Result is ObjectResult)
            {
                var resp = (ObjectResult)context.Result;
                if (resp != null)
                    cacheManager.SetCache(key, resp.Value, TimeSpan.FromSeconds(Duration));
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var cacheManager = context.HttpContext.RequestServices.GetService<ICacheManager>();
            var key = FindRedisCacheKey(context.HttpContext);
            var data = cacheManager.GetCache(key);
            if (data is null) return;
            var objResult = new ObjectResult(data);
            if (data != null)
                context.Result = objResult;
        }

        private string FindRedisCacheKey(HttpContext context)
        {
            var encodedUrl = context.Request.Path.Value;
            var redisCacheKey = RedisHelper.QueryStringKeyGenerator(context.Request.Query);
            var key = encodedUrl + redisCacheKey;
            return key;
        }
    }
}
