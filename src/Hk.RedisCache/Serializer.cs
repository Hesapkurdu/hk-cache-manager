using Hk.RedisCache.Contracts;
using Newtonsoft.Json;

namespace Hk.RedisCache
{
    public class Serializer : ISerializer
    {
        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
