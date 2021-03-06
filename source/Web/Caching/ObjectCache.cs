﻿using System;
using System.Web;
using SiSystems.ClientApp.Web.Domain.Caching;

namespace SiSystems.ClientApp.Web.Caching
{
    public class ObjectCache : IObjectCache
    {
        public void AddItem(string key, object value)
        {
            var minutesToCache = Settings.ObjectCacheLifetime;
            var cacheExpirationTime = DateTime.Now.AddMinutes(minutesToCache);

            HttpRuntime.Cache.Insert(key, value, null, cacheExpirationTime, System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public object GetItem(string key)
        {
            var value = HttpRuntime.Cache.Get(key);
            return value;
        }
    }
}
