using System;
using System.Collections.Generic;
using System.Text;
using NoSql.Redis;
using NUnit.Framework;

namespace Test.NUnit
{
    [TestFixture]
    public class RedisTest
    {
        private ICache _cacheManager;

        [SetUp]
        public void Setup()
        {
            _cacheManager = new RedisService();
        }

        [Test]
        public void TestString()
        {
            Assert.IsTrue(_cacheManager.Add("key1", "value1", 100 * 1000));
            Assert.NotNull(_cacheManager.Get("key1"));
            Assert.IsTrue(_cacheManager.Remove("key1"));
        }

    }
}
