using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public static class KeyGenerator
    {
        public static string NextVersion() {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}