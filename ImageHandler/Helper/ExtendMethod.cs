using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace ImageHandler.Helper
{
    public static class ExtendMethod
    {
        public static bool ContainsKey(this NameValueCollection collection, string key)
        {
            if (collection.Get(key) == null)
            {
                return collection.AllKeys.Contains(key);
            }

            return true;
        }
    }
}