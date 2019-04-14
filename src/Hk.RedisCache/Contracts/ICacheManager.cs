using JetBrains.Annotations;
using System;

namespace Hk.RedisCache.Contracts
{
    public interface ICacheManager
    {
        string GetCache([NotNull] string name);
        T GetCache<T>([NotNull] string name);
        T GetCache<T>([NotNull] T @object);
        void SetCache([NotNull] string name, [NotNull] string value);
        void SetCache([NotNull] string name, [NotNull] string value, [NotNull] TimeSpan timeSpan);
        void SetCache<T>([NotNull] T value);
        void SetCache<T>([NotNull] string name, [NotNull] T value);
        void SetCache<T>([NotNull] string name, [NotNull] T value, [NotNull] TimeSpan timeSpan);

        T GetFromCacheOrRun<T>([NotNull]string cacheKey, Func<T> action);
    }
}
