using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{


    /// <summary>
    /// 验证多个属性至少有一个为必填
    /// </summary>
    
    public class MultiRequiredAttribute : ValidationAttribute
    {

        /// <summary>
        /// 包含的字段
        /// </summary>
        
        public string Includes;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {

            return base.IsValid(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string[] includes = Includes.Split(',');
            string name = "";
            int i = 0;

            foreach (var f in includes)
            {
                PropertyInfo property = validationContext.ObjectType.GetProperty(f);
                if (property == null)
                {
                    return new ValidationResult(string.Format(" '{0}' 字段未定义。", f), includes);
                }
                //取要比较的字段显示名
                var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(null, validationContext.ObjectType,f);
                string propertyDisplayName = "";
                if (metadata != null)
                {
                    propertyDisplayName = metadata.DisplayName;
                    if (metadata.DisplayName == null)
                        propertyDisplayName = f;
                }
                name += "'" + propertyDisplayName + "'";
                var fieldValue = property.GetValue(validationContext.ObjectInstance, null);
                if (fieldValue == null)
                {
                    i = i + 1;
                }
            }

            if (i >= includes.Length)
            {
                
                return new ValidationResult(string.Format("{0} 至少填写一项。", name), includes.AsEnumerable());
            }
            return ValidationResult.Success;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
    }
}
