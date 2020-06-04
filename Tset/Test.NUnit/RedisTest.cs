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
            string key = "key1";
            if (_cacheManager.Exist(key))
            {
                Assert.IsTrue(_cacheManager.Remove(key));
            }
            Assert.IsTrue(_cacheManager.Add(key, "value1", 100 * 1000));
            Assert.NotNull(_cacheManager.Get(key));
            Assert.IsTrue(_cacheManager.Remove(key));
        }

    }
}
