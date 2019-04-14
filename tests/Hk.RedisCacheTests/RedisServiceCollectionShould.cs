using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Hk.RedisCache.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Hk.RedisCacheTests
{
    public class ServiceCollectionTests : IClassFixture<TestFixture<FakeStartup>>
    {
        private readonly TestFixture<FakeStartup> _fixture;

        public ServiceCollectionTests(TestFixture<FakeStartup> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void StartSuccessfully_With_RedisServices()
        {
            //Act
            var type = _fixture.Services.GetService(typeof(IHkRedisCacheDatabaseProvider));

            //Assert
            type.Should().NotBeNull();
        }

        [Fact]
        public void StartSuccessfully_With_CacheManager()
        {
            //Act
            var type = _fixture.Services.GetService(typeof(ICacheManager));
            
            //Assert
            type.Should().NotBeNull();
        }

        [Fact]
        public void StartSuccessfully_With_Serializer()
        {
            //Act
            var type = _fixture.Services.GetService(typeof(ISerializer));

            //Assert
            type.Should().NotBeNull();
        }

        [Fact]
        public void ReturnDefaultTimeout_When_DefaultInvoke()
        {
            //Act
            var type = _fixture.Services.GetService<IHkRedisCacheDatabaseProvider>();
            
            //Assert
            type.Timeout.Should().Be(TimeSpan.FromHours(1));
        }
    }
}
