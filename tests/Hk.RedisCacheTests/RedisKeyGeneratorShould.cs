using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Hk.RedisCache;
using Hk.RedisCacheTests.Models;
using Xunit;

namespace Hk.RedisCacheTests
{
    public class RedisKeyGeneratorShould
    {
        [Fact]
        public void ReturnValidKey_When_ParameterObjectIsDefault()
        {
            //Arrange
            var customer = new Customer();

            //Assert
            var sut = RedisHelper.KeyGenerator(customer);

            //Assert
            sut.Should().Be($"{nameof(Customer)}:{default(int)}");
        }

        [Fact]
        public void ReturnValidKey_When_ParameterObjectIsPopulated()
        {
            //Arrange
            var fixture = new Fixture();
            var customer = fixture.Build<Customer>().Create();

            //Assert
            var sut = RedisHelper.KeyGenerator(customer);

            //Assert
            sut.Should().Be($"{nameof(Customer)}:{customer.Id}");
        }

        [Fact]
        public void ReturnValidKey_When_ParameterObjectIdIsNegativeValue()
        {
            //Arrange
            var fixture = new Fixture();
            var customer = fixture.Build<Customer>()
                .With(x => x.Id, -99).Create();

            //Assert
            var sut = RedisHelper.KeyGenerator(customer);

            //Assert
            sut.Should().Be($"{nameof(Customer)}:{customer.Id}");
        }

        [Fact]
        public void ReturnValidKey_When_IdObjectIsString()
        {
            //Arrange
            var fixture = new Fixture();
            var member = fixture.Build<Member>()
             .Create();
            var memberId = string.Concat(member.Id.Select((x, i) => i > 0 && !char.IsLetterOrDigit(x) ? "" : x.ToString())).ToLower();

            //Act
            var sut = RedisHelper.KeyGenerator(member);
            //Assert
            sut.Should().Be($"{nameof(Member)}:{memberId}");
        }

        [Fact]
        public void ThrowException_When_IdObjectIsNull()
        {
            //Arrange
            var member = new Member();
            member.Id = null;
            
            //Assert
            Assert.Throws<ArgumentException>(() => RedisHelper.KeyGenerator(member));
        }

        [Fact]
        public void ReturnValidKey_When_IdObjectIsGuid()
        {
            //Arrange
            var fixture = new Fixture();
            var member = fixture.Build<MemberWithGuid>()
                .Create();
            var memberId = string.Concat(member.Id.ToString().Select((x, i) => i > 0 && !char.IsLetterOrDigit(x) ? "" : x.ToString())).ToLower();

            //Assert
            var sut = RedisHelper.KeyGenerator(member);


            //Assert
            sut.Should().Be($"{nameof(MemberWithGuid)}:{memberId}");
        }

        [Fact]
        public void ThrowsArgumentNullException_When_ObjectIsNull()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() => RedisHelper.KeyGenerator<Member>(null));
        }
    }

}
