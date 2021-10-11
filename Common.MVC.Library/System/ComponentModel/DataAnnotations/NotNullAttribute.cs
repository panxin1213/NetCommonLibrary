using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections;

namespace System.ComponentModel.DataAnnotations
{
    public class NotNullAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// 参数指定
        /// </summary>
        /// <param name="maximumLength"></param>
        public NotNullAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            return true;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null || (value as ICollection).Count == 0)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            return null;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("{0}不能为空", name);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return null;
        }

    }
}
