using System.Threading.Tasks;

namespace NoSql.Redis
{
    /// <summary>
    /// https://github.com/SkyChenSky/Sikiro.Tookits/blob/master/Sikiro.Tookits/Interfaces/ICache.cs
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        bool Add(string key, string value, int seconds = 0);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        bool Add<T>(string key, T value, int seconds = 0) where T : class, new();
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        Task<bool> AddAsync(string key, string value, int seconds = 0);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        Task<bool> AddAsync<T>(string key, T value, int seconds = 0) where T : class, new();

        /// <summary>
        /// 取
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        string Get(string key);
        /// <summary>
        /// 取
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        T Get<T>(string key) where T : class;
        /// <summary>
        /// 取
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<string> GetAsync(string key);
        /// <summary>
        /// 取
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key) where T : class;

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool Exist(string key);
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">键</param>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool Remove(string key);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(string key);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys">键</param>
        /// <returns></returns>
        Task<long> RemoveAsync(string[] keys);
    }
}
