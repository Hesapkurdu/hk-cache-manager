using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Hk.RedisCache
{
    public static class RedisHelper
    {
        public static string KeyGenerator<T>(T @object)
        {
            if (@object is null) throw new ArgumentNullException($"{nameof(@object)} is null! Cannot generate key!");
            var type = @object.GetType();
            var idProperty = type.GetProperty("Id");
            var idPropertyValue = idProperty.GetValue(@object);
            if (idPropertyValue is null)
                throw new ArgumentException($"{type.Name} should have Id Property! Otherwise please use other overload methods!");
            var idPropertTrimmedLowerCaseValue = idPropertyValue.ToString().Trim().ToLowerInvariant();
            var snakeCase = string.Concat(idPropertTrimmedLowerCaseValue.Select((x, i) => i > 0 && char.IsWhiteSpace(x) ? "_" + x.ToString() : x.ToString()));
            var onlyLetterOrDigit = string.Concat(snakeCase.Select((x, i) => i > 0 && !char.IsLetterOrDigit(x) ? "" : x.ToString()));
            var onlyLetterOrDigitName = string.Concat(type.Name.Select((x, i) => i > 0 && !char.IsLetterOrDigit(x) ? "" : x.ToString()));
            var name = $"{onlyLetterOrDigitName}:{onlyLetterOrDigit}";
            return name;
        }

        public static string QueryStringKeyGenerator(IQueryCollection queryDictionary)
        {
            var redisCacheKey = string.Empty;
            if (queryDictionary != null)
                foreach (var query in queryDictionary)
                    redisCacheKey += $"_{query.Key}_{query.Value}";
            return redisCacheKey;
        }
    }
}
