using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.App_Start
{
    /// <summary>
    /// Override for DefaultModelBinder in order to implement fixes to its behavior.
    /// This model binder inherits from the default model binder. All this does is override the default one,
    /// check if the property is an enum, if so then use custom binding logic to correctly map the enum. If not,
    /// we simply invoke the base model binder (DefaultModelBinder) and let it continue binding as normal.
    /// </summary>
    public class EnumModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Fix for the default model binder's failure to decode enum types when binding to JSON.
        /// </summary>
        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            var propertyType = propertyDescriptor.PropertyType;
            if (propertyType.IsEnum)
           
            {
                var providerValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (null != providerValue)
                {
                    var value = providerValue.RawValue;
                    if (null != value)
                    {
                        var valueType = value.GetType();
                        if (!valueType.IsEnum)
                        {
                            return Enum.ToObject(propertyType, value);
                        }
                    }
                }
            }
            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }
    }
}