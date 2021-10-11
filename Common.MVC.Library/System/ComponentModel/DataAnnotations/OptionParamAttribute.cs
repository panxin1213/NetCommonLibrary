using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Configuration;
using ChinaBM.Common;
using System.Collections;

namespace System.ComponentModel.DataAnnotations
{
    public class OptionParamAttribute : ValidationAttribute, IClientValidatable
    {
        public Dictionary<string, string> Params { get; set; }

        public string GetValue(IEnumerable<string> keys)
        {
            return string.Join(",", Params.Where(a => keys.Any(b => a.Key == b)).Select(a => a.Value));
        }


        /// <summary>
        /// 参数指定
        /// </summary>
        /// <param name="maximumLength"></param>
        public OptionParamAttribute(string appSettingsKey)
        {
            if (String.IsNullOrEmpty(appSettingsKey) || String.IsNullOrEmpty(ConfigurationManager.AppSettings[appSettingsKey]))
            {
                throw new Exception("没有在web.config的AppSettings关于\"" + appSettingsKey + "\"配置地址。");
            }

            if (ConfigurationManager.AppSettings[appSettingsKey].IndexOf(",") > -1 && ConfigurationManager.AppSettings[appSettingsKey].IndexOf(".xml") < 0)
            {
                Params = ConfigurationManager.AppSettings[appSettingsKey].Split(',').ToDictionary(a => a, a => a);
            }
            else
            {
                try
                {
                    var document = XElement.Load(HttpKit.GetMapPath(Configuration.ConfigurationManager.AppSettings[appSettingsKey]));
                    Params = document.Descendants("item").Select(a => new KeyValuePair<string, string>(a.Attribute("ID") == null ? a.Value : a.Attribute("ID").Value, a.Value)).ToDictionary(a => a.Key, a => a.Value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            return Params.Any(a => a.Equals(value.ToString()));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            var result = true;

            if (value != null)
            {
                var mm = value as IEnumerable;

                if (mm != null)
                {
                    result = !mm.Cast<object>().All(a => Params.Any(b => a.ToString() == b.Key.Trim().ToString()));
                }

                if (result)
                {
                    result = !Params.Any(a => a.Key.Equals(value.ToString()));
                }
            }
            else
            {
                result = false;
            }

            if (result)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            return null;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("{0}必须为({1})中一个", name, string.Join(",", Params.Select(a => a.Key)));
        }

        #region IClientValidatable 成员

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule validationRule = new ModelClientValidationRule()
            {
                ValidationType = "regex",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),

            };
            validationRule.ValidationParameters.Add("pattern", string.Join("|", Params.Select(a => a.Key)));
            yield return validationRule;
        }

        #endregion
    }
}
