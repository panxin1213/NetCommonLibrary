using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{
    public class EqualToAttribute : ValidationAttribute, IClientValidatable
    {
        private string _otherName = "";

        /// <summary>
        /// 其他字段displayName
        /// </summary>
        private string _otherDisplayName = "";

        public EqualToAttribute(string otherName)
        {
            _otherName = otherName;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("{0} 必须和 {1} 值一样。", name, !String.IsNullOrEmpty(_otherDisplayName) ? _otherDisplayName : _otherName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var t = validationContext.ObjectInstance.GetType();

            var p = t.GetProperties().SingleOrDefault(a => a.Name.Equals(_otherName));

            if (p != null)
            {
                var o = p.GetValue(validationContext.ObjectInstance, null);
                if (o == null || (o != null && value != null && o.ToString() == value.ToString()))
                {
                    return null;
                }

                if (String.IsNullOrEmpty(ErrorMessage))
                {
                    var display = p.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                    if (display != null && !String.IsNullOrEmpty(display.Name))
                    {
                        _otherDisplayName = display.Name;
                    }

                    ErrorMessage = FormatErrorMessage(validationContext.DisplayName);
                }

                return new ValidationResult(ErrorMessage);
            }

            return null;
        }



        #region IClientValidatable 成员

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule()
            {
                ValidationType = "equalto",
                ErrorMessage = !String.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : FormatErrorMessage(metadata.DisplayName),

            };
            validationRule.ValidationParameters.Add("other", _otherName);
            yield return validationRule;
        }

        #endregion
    }
}
