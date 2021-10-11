using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Mvc;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 关于返回值形式的枚举
    /// </summary>
    public enum CompareAction
    {

        /// <summary>
        /// 大于
        /// </summary>
        [Description("大于")]
        Greater,
        /// <summary>
        /// 等于
        /// </summary>
        [Description("等于")]
        Equal,
        /// <summary>
        /// 小于
        /// </summary>
        [Description("小于")]
        Less,
        /// <summary>
        /// 大于或等于
        /// </summary>
        [Description("大于或等于")]
        GreaterOrEqual,
        /// <summary>
        /// 小于或等于
        /// </summary>
        [Description("小于或等于")]
        LessOrEqual
    }

    /// <summary>
    /// 
    /// </summary>
    public class CompareField
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="action"></param>
        public CompareField(string field, CompareAction action)
        {
            Field = field;
            Action = action;
        }
        /// <summary>
        /// 要比较的字段
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 要比较的类型
        /// </summary>
        public CompareAction Action { get; set; }
    }
    /// <summary>
    /// 比较两个属性值的大小
    /// </summary>
    public class CompareValueAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public CompareValueAttribute()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields"></param>
        public CompareValueAttribute(CompareField[] fields)
        {
            Fields = fields;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="action"></param>
        public CompareValueAttribute(string field, CompareAction action)
        {
            Fields = new CompareField[]
            {
                new CompareField(field,action)
            };
        }

        private CompareField[] Fields;

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fields"></param>
        /*
        public CompareValue(string fields)        
        {
            _fields = fields;         
        }*/
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
            foreach (var f in Fields)
            {
                
                PropertyInfo property = validationContext.ObjectType.GetProperty(f.Field);
                if (property == null)
                {
                    return new ValidationResult(string.Format(" '{0}' 未定义。", f.Field));
                }
                //取要比较的字段显示名
                var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(null, validationContext.ObjectType,f.Field);
                string propertyDisplayName = f.Field;
                if (metadata != null)
                {
                    propertyDisplayName = metadata.DisplayName;
                }
                
                var fieldValue = property.GetValue(validationContext.ObjectInstance, null);
                
                if (fieldValue == null)
                {
                    return new ValidationResult(string.Format("'{0}' 不能为空。", propertyDisplayName));
                }
                if (value==null)
                {
                    return new ValidationResult(string.Format("'{0}' 不能为空。", validationContext.DisplayName));
                }
                var r = GetCompareValue(f.Action, fieldValue, value);
                if (r == false)
                {
                    return new ValidationResult(ErrorMessage ?? string.Format("'{0}' 必须{1} '{2}'。", validationContext.DisplayName, f.Action.GetDescription(), propertyDisplayName));
                }
            }
            return null;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public override string FormatErrorMessage(string name)
        //{
        //    return base.FormatErrorMessage(name);
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">要比较的；类型</param>
        /// <param name="field">要比较的属性</param>
        /// <param name="value">当前属性</param>
        /// <returns></returns>
        private bool GetCompareValue(CompareAction type, object field, object value)
        {
            var f = field as IComparable;
            var v = value as IComparable;
            if (f == null || v == null)
                return false;
            switch (type)
            {
                case CompareAction.Greater:
                    {
                        return v.CompareTo(f) > 0;
                        //return (v-f).Days>0;
                    }
                case CompareAction.Equal:
                    {
                        return v.CompareTo(f) == 0;
                        //return (value.ToInt32() - field.ToInt32()) == 0;
                    }
                case CompareAction.Less:
                    {
                        return v.CompareTo(f) < 0;
                        //return (value.ToInt32() - field.ToInt32()) < 0;
                    }
                case CompareAction.GreaterOrEqual:
                    {
                        return v.CompareTo(f)>=0;
                    }
                case CompareAction.LessOrEqual:
                    {
                        return v.CompareTo(f)<=0;
                    }

            }
            return false;
        }
    }
}
