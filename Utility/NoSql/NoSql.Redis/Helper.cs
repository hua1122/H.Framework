using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NoSql.Redis
{
    internal class Helper
    {

        internal static string PackKey(string key)
        {
            return RedisContext._keyPrefix + key;
        }

        internal static TimeSpan? SecondToTimeSpan(int seconds = 0)
        {
            TimeSpan? ts = null;
            if (seconds > 0)
            {
                ts = new TimeSpan(0, 0, seconds);
            }
            return ts;
        }

        internal static string ObjectToString(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            else
            {
                return JsonConvert.SerializeObject(obj);
            }
        }

        internal static T StringToObject<T>(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
        }

    }
}
