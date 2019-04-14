using System;
using Hk.RedisCache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.AspNetCore.Models;

namespace Sample.AspNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkInMemoryDatabase().AddMvc();
            
            services.AddHkRedisCache(new HkRedisOptions
            {
                ConnectionString = "localhost:6379",
                DatabaseId = 1
            });
            services.AddDbContextPool<ApplicationDbContext>(x => x.UseInMemoryDatabase());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseMvc();
            var db = serviceProvider.GetService<ApplicationDbContext>();
            db.Customers.Add(new Customer() { Id = 1, Name = "Orhun" });
            db.SaveChanges();
        }
    }
}
