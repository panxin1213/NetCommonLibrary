using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    /// <summary>
    /// 字符类型自动Trim
    /// </summary>
    public class StringTrimModelBinder : DefaultModelBinder 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = base.BindModel(controllerContext, bindingContext);
            if (value is string) return (value as string).Trim();
            return value;
        }
    }
}

/// <summary>
/// 
/// </summary>
public class TrimModelBinder : DefaultModelBinder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerContext"></param>
    /// <param name="bindingContext"></param>
    /// <returns></returns>
    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        var value = base.BindModel(controllerContext, bindingContext);
        if (value is string) return (value as string).Trim();
        return value;
    }
    /*
    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerContext"></param>
    /// <param name="bindingContext"></param>
    /// <param name="propertyDescriptor"></param>
    /// <param name="value"></param>
    protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
    {
        if (propertyDescriptor.PropertyType == typeof(string))
        {
            var stringValue = (string)value; if (!string.IsNullOrEmpty(stringValue))
                stringValue = stringValue.Trim(); value = stringValue;
        }
        base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
    }*/
}