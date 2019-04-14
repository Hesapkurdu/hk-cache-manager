using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Hk.RedisCache;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Hk.RedisCacheTests
{
    public class RedisQueryStringKeyGeneratorShould
    {
        [Fact]
        public void ReturnEmptyKey_With_QueryStringList()
        {
            //Arrange
            var queryDict = new QueryCollection();

            //Act
            var sut = RedisHelper.QueryStringKeyGenerator(queryDict);

            //Assert
            sut.Should().BeEmpty();
        }

        [Fact]
        public void ReturnEmptyKey_With_Null()
        {
            //Act
            var sut = RedisHelper.QueryStringKeyGenerator(null);

            //Assert
            sut.Should().BeEmpty();
        }

        [Fact]
        public void ReturnGeneratedKey_With_PopulatedQueryString()
        {
            //Arrange
            var queryDict = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"id","123" }
            });

            //Act
            var sut = RedisHelper.QueryStringKeyGenerator(queryDict);

            //Assert
            sut.Should().Be("_id_123");
        }
    }
}
