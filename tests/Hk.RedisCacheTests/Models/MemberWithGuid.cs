using System;

namespace Hk.RedisCacheTests.Models
{
    public class MemberWithGuid
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}