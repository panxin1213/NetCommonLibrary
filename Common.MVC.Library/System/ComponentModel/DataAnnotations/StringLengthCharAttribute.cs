using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Varchar长度判断
    /// </summary>
    public class StringLengthCharAttribute : ValidationAttribute ,IClientValidatable
    {
        int _maxLength = 0;
        /// <summary>
        /// 参数指定
        /// </summary>
        /// <param name="maximumLength"></param>
        public StringLengthCharAttribute(int maximumLength)
        {
            _maxLength = maximumLength;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {

            return GetLength(value)<= _maxLength;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var l = GetLength(value);
            if (l > _maxLength)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int GetLength(object value)
        { 
            if (value == null) return 0;
            return System.Text.Encoding.Default.GetBytes(value.ToString()).Length;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format("{0} 必须是最大长度为 {1} 字节(中文2字节)", name, _maxLength);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule()
            {
                ValidationType = "len",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),

            };
            validationRule.ValidationParameters.Add("max", _maxLength);
            yield return validationRule;
        }
    }
}
/*
//字节长度
function getLenByte(v) {
    var cArr = v.match(/[^\x00-\xff]/ig);         
    return v.length + (cArr == null ? 0 : cArr.length);
}
//字节长度验证
jQuery.validator.addMethod("len", function (value, element, params) {
    return (getLenByte(value) <= params.max);
})
jQuery.validator.unobtrusive.adapters.add("len", ["max"], function (options) {
    options.rules["len"] = {
       max: options.params.max
    };
   options.messages["len"] = options.message;
});
*/