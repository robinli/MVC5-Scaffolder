using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class MyDictionary
    {
        public static Dictionary<int, string> MessageGroup =
     new Dictionary<int, string> {
        { 0, "系统操作" },
        { 1, "业务操作" },
            { 2,"接口操作"} };

        public static Dictionary<int, string> MessageType =
      new Dictionary<int, string> {
        { 0, "Information" },
        { 1, "Error" },
            { 2,"Alert"} };
    }

   
}