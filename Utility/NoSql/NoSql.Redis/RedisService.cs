using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace NoSql.Redis
{
    public class RedisService : ICache
    {
        #region 连接对象

        private IDatabase Db => RedisContext.Instance.GetDatabase();

        #endregion

        #region Key操作
        public bool Exist(string key)
        {
            return Do(redis => redis.KeyExists(key));
        }
        public async Task<bool> ExistsAsync(string key)
        {
            return await Do(async redis => await redis.KeyExistsAsync(key));
        }

        public bool Remove(string key)
        {
            return Do(redis => redis.KeyDelete(key));
        }
        public async Task<bool> RemoveAsync(string key)
        {
            return await Do(async redis => await redis.KeyDeleteAsync(key));
        }
        public async Task<long> RemoveAsync(string[] keys)
        {
            var removed = 0L;

            foreach (var key in keys)
            {
                if (await RemoveAsync(key))
                    removed++;
            }
            return removed;
        }

        #endregion

        public bool Add(string key, string value, int seconds = 0)
        {
            return Do(redis => redis.StringSet(key, value, Helper.SecondToTimeSpan(seconds), When.NotExists));
        }
        public bool Add<T>(string key, T value, int seconds = 0) where T : class, new()
        {
            return Add(key, Helper.ObjectToString(value), seconds);
        }
        public async Task<bool> AddAsync(string key, string value, int seconds = 0)
        {
            return await Do(async redis => await redis.StringSetAsync(key, value, Helper.SecondToTimeSpan(seconds), When.NotExists));
        }
        public async Task<bool> AddAsync<T>(string key, T value, int seconds = 0) where T : class, new()
        {
            return await AddAsync(key, Helper.ObjectToString(value), seconds);
        }

        public string Get(string key)
        {
            return Do(redis => redis.StringGet(key));
        }
        public async Task<string> GetAsync(string key)
        {
            return await Do(async redis => await redis.StringGetAsync(key));
        }
        public T Get<T>(string key) where T : class
        {
            return Helper.StringToObject<T>(Get(key));
        }
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            return Helper.StringToObject<T>(await GetAsync(key));
        }

        private void Do(Action<IDatabase> action)
        {
            action(Db);
        }
        private T Do<T>(Func<IDatabase, T> func)
        {
            return func(Db);
        }

    }
}
