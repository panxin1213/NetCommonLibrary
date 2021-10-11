using System.Collections.Generic;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 必须为true的验证
    /// </summary>
    public class MustBeTrueAttribute : ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            var boolValue = value as bool?;
            if (boolValue.HasValue && boolValue.Value) return true;
            return false;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var validationRule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "mustbetrue"
            };
            yield return validationRule;
        }
    }
}
