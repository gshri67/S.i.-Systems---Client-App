using System;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Cache
{
    public class ObjectCache : IObjectCache
    {
        private static readonly System.Web.Caching.Cache Cache = new System.Web.Caching.Cache();

        public void AddItem(string key, object value)
        {
            var minutesToCache = Settings.ObjectCacheLifetime;
            var cacheExpirationTime = DateTime.Now.AddMinutes(minutesToCache);

            Cache.Insert(key, value, null, cacheExpirationTime, System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public object GetItem(string key)
        {
            var value = Cache.Get(key);
            return value;
        }
    }
}
