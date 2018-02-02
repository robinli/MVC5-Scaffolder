using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using WebApp.Models;
namespace WebApp
{
    public class InMemoryCache : ICacheService
    {

        public TValue Get<TValue>(string cacheKey, Func<TValue> getItemCallback) where TValue : class
        {
            int durationInMinutes = 10;
            TValue item = MemoryCache.Default.Get(cacheKey) as TValue;
            if (item == null)
            {
                item = getItemCallback();
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddMinutes(durationInMinutes));
            }
            return item;
        }

        public TValue Get<TValue, TId>(string cacheKeyFormat, TId id, Func<TId, TValue> getItemCallback) where TValue : class
        {
            int durationInMinutes = 10;
            string cacheKey = string.Format(cacheKeyFormat, id);
            TValue item = MemoryCache.Default.Get(cacheKey) as TValue;
            if (item == null)
            {
                item = getItemCallback(id);
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddMinutes(durationInMinutes));
            }
            return item;
        }


    }

    public interface ICacheService
    {
        TValue Get<TValue>(string cacheKey, Func<TValue> getItemCallback) where TValue : class;
        TValue Get<TValue, TId>(string cacheKeyFormat, TId id, Func<TId, TValue> getItemCallback) where TValue : class;
    }



}