[![Build status](https://ci.appveyor.com/api/projects/status/maxyeonv3cal84o9?svg=true)](https://ci.appveyor.com/project/obegendi/hkcachemanager)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Hesapkurdu_HkCacheManager&metric=alert_status)](https://sonarcloud.io/dashboard?id=Hesapkurdu_HkCacheManager)
# Hesapkurdu.com Cache Manager
Easy usage & cleaner code with cache related code.

This project aims developers who use Redis as Cache Server. Provides easy API response cache, Entity/complex object cache, simple cache structure for developers.

It is simply a wrapper for Stackexchange.Redis and AspNetCore Service & ActionFilter.

# Overview
This package can help caching responses and complex object cleaner! Here is simple example:

```c#
    [HttpGet]
    [RedisResponseCache]
    public ActionResult<IEnumerable<string>> Get()
    {
        return new[] { "value1", "value2" };
    }
```

This example shows how easily response cache with Redis.

You can easily use this feature. Just need add to AspNetCore Services.

```c#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        //Order not important
        services.AddMvc();

        services.AddHkRedisCache(new HkRedisOptions
        {
            ConnectionString = "localhost:6379",
            DatabaseId = 1,
            Timeout = TimeSpan.FromHours(1) //(default value of cache timeout)
        });
    }
}
```

Also it provides complex object caching.

```c#
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var data = _cacheManager.Get<Customer>($"{nameof(Customer)}:{id}");
        if(data == null)
          return Ok(_context.Customers.FirstOrDefault(x => x.Id == id));
        else
        return Ok(data);
    }
```

Instead of this code you can use easily example down below.

```c#
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = _cacheManager.GetFromCacheOrRun($"{nameof(Customer)}:{id}", () => { return _context.Customers.FirstOrDefault(x => x.Id == id); });
        return Ok(result);
    }
```

This simple usage cache any function response. It is useful for DAL return caching. You can use for every function! (Except void)

Also provides simple caching with Cache Manager object (ICacheManager)

```c#
public class CustomerController : ControllerBase
    {
        private readonly ICacheManager _cacheManager;
        private readonly ApplicationDbContext _context;

        public CustomerController(ICacheManager cacheManager, ApplicationDbContext context)
        {
            _cacheManager = cacheManager;
            _context = context;
        }

        [HttpGet("cache/{id}")]
        public async Task<IActionResult> GetFromCache([FromRoute]int id)
        {
            var result = _context.Customers.FirstOrDefault(x => x.Id == id);
            _cacheManager.SetCache(result);
            return Ok(result);
        }
```

Also have some overload for simple usage

```c#
_cacheManager.SetCache(customerObject);
_cacheManager.SetCache("RedisKey",customerObject);
_cacheManager.SetCache("RedisKey",stringObject);
```

Cache Keys for objects
```c#
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
}

var customer = new Customer(){Id = 1, Name="Orhun"}
_cacheManager.SetCache(customerObject);
//key will be "Customer:1_Orhun"
```

Cache Keys will be every value with "_" (underscore) seperated. ":" (colon) for Redis default foldering mark.


# Build
Install Visual Studio 2017 & .Net Core 2.1+ and run!

`build.ps1`

# Who uses this package?
Hesapkurdu.com R&D team using at production!
