using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp
{
    public class ButtonAttribute : ActionMethodSelectorAttribute
    {
        public string Name { get; set; }
        public ButtonAttribute(string name) => this.Name = name;
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo) => controllerContext.Controller.ValueProvider.GetValue(Name) != null;
    }
}