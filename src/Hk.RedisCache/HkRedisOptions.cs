using JetBrains.Annotations;
using System;

namespace Hk.RedisCache
{
    public class HkRedisOptions
    {
        /// <summary>
        ///     Redis Database Id. Default is 0
        /// </summary>
        public int DatabaseId { get; set; }
        /// <summary>
        ///     Redis Connection string. Should be not null! Not tested on HTTPS!
        ///     <example>localhost:6379</example>
        /// </summary>
        [NotNull]
        public string ConnectionString { get; set; }
        /// <summary>
        ///     Cache duration/timeout. Default is 1 Hour.
        /// </summary>
        public TimeSpan Timeout { get; set; }
    }
}
