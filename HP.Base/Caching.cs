namespace HP.Base;

/// <summary>
    /// Caching缓存类
    /// </summary>
    public class Caching
    {
        /// <summary>
        /// 设置指定CacheKey的Cache值,无过期时间
        /// </summary>
        /// <param name="CacheKey">
        /// <param name="objObject">
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }

        /// <summary>
        /// 设置指定CacheKey的Cache值和有效时长
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="CachValue"></param>
        /// <param name="seconds">超过多少秒后过期</param>
        public static void SetCache(string CacheKey, object CachValue, long Seconds)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, CachValue, null, System.DateTime.Now.AddSeconds(Seconds), TimeSpan.Zero, CacheItemPriority.High, null);
        }


        /// <summary>
        /// 获取当前指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey">
        /// <returns></returns>y
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 清除某一个Key的缓存
        /// </summary>
        /// <param name="key">
        public static void RemoveKeyCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Remove(CacheKey);
        }

        /// <summary>  
        /// 清除所有缓存
        /// </summary>  
        public static void RemoveAllCache1()
        {
            var cache = HttpRuntime.Cache;
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }

        /// <summary>
        /// 以列表形式返回已存在的所有缓存 
        /// </summary>
        /// <returns></returns> 
        public static ArrayList ShowAllCache()
        {
            ArrayList al = new ArrayList();
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            if (_cache.Count > 0)
            {
                IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
                while (CacheEnum.MoveNext())
                {
                    al.Add(CacheEnum.Key);
                }
            }
            return al;
        }
    }
