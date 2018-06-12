using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ButtonAttribute : ActionMethodSelectorAttribute
    {
        public string Name { get; set; }
        public ButtonAttribute(string name)
        {
            Name = name;
        }
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            return controllerContext.Controller.ValueProvider.GetValue(Name) != null;
        }
    }
}