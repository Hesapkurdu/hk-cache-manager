using System.Collections.Generic;
using Hk.RedisCache;
using Hk.RedisCache.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Sample.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ICacheManager _cacheManager;

        public ValuesController(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        // GET api/values
        [HttpGet]
        [RedisResponseCache]
        public ActionResult<IEnumerable<string>> Get()
        {
            //_cacheManager.SetCache("test");
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            _cacheManager.SetCache<object>(new { Id = "test112" });
            return _cacheManager.GetCache("test");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
