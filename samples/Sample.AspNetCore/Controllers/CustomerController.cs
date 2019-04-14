using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hk.RedisCache;
using Hk.RedisCache.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.AspNetCore.Models;

namespace Sample.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICacheManager _cacheManager;
        private readonly ApplicationDbContext _context;
        public CustomerController(ICacheManager cacheManager, ApplicationDbContext context)
        {
            _cacheManager = cacheManager;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = _cacheManager.GetFromCacheOrRun($"{nameof(Customer)}:{id}", () => { return _context.Customers.FirstOrDefault(x => x.Id == id); });
            return Ok(result);
        }

        [HttpGet("cache/{id}")]
        [RedisResponseCache()]
        public async Task<IActionResult> GetFromCache([FromRoute]int id)
        {
            var result = _context.Customers.FirstOrDefault(x => x.Id == id);
            return Ok(result);
        }

    }
}