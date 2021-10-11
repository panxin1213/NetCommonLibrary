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
    public enum DateTimeRangeType
    {

        /// <summary>
        /// 年数
        /// </summary>
        Year,
        /// <summary>
        /// 月数
        /// </summary>
        Month,
        /// <summary>
        /// 天数
        /// </summary>
        Day,
        /// <summary>
        /// 分钟
        /// </summary>
        Minute
    }
    /// <summary>
    /// 时间范围限制(相对当前时间)
    /// </summary>
    public class DateTimeRange : ValidationAttribute
    {
        private DateTimeRangeType _type = DateTimeRangeType.Day;
        /// <summary>
        /// 可选
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        /// 可选
        /// </summary>
        public int Max { get; set; }
        /// <summary>
        /// 可选 默认DateTimeRangeType
        /// </summary>
        public DateTimeRangeType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        /// <summary>
        /// 要参照的字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
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
            if (!(value is DateTime))
            {
                throw new Exception("字段必须为时间类型");
            }
            DateTime fieldValue;
            string propertyDisplayName;
            if (!String.IsNullOrEmpty(Field))
            {
                PropertyInfo property = validationContext.ObjectType.GetProperty(Field);
                if (property == null)
                {
                    return new ValidationResult(string.Format(" '{0}' 字段未定义。", Field));
                }
                //取要比较的字段显示名
                var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(null, validationContext.ObjectType, Field);

                if (metadata != null)
                {
                    propertyDisplayName = metadata.DisplayName;
                }
                else
                {
                    propertyDisplayName = Field;
                }
                fieldValue = (DateTime)property.GetValue(validationContext.ObjectInstance, null);
            }
            else
            {
                fieldValue = DateTime.Now;
                propertyDisplayName = validationContext.DisplayName;
            }

            if (fieldValue == null)
            {
                return new ValidationResult(string.Format("'{0}' 字段的值为空或不存在。", propertyDisplayName));
            }
            var dateValue = (DateTime)value;
            if (Max == int.MaxValue)
            {
                DateTime minDateTime = GetDateTime(fieldValue, Min);
                if (dateValue < minDateTime)
                    return new ValidationResult(string.Format("'{0}'必须大于等于{1}。", validationContext.DisplayName, FormatDate(minDateTime)));
            }
            if ( Min == int.MinValue)
            {
                DateTime maxDateTime = GetDateTime( fieldValue, Max);
                if (dateValue > maxDateTime)
                    return new ValidationResult(string.Format("'{0}'必须小于等于{1}。", validationContext.DisplayName, FormatDate(maxDateTime)));
            }
            //if (Min != 0 && Max != 0)
            //{
            //    DateTime minDateTime = GetDateTime(_type, fieldValue, Min);
            //    DateTime maxDateTime = GetDateTime(_type, fieldValue, Max);
            //    if (dateValue < minDateTime || dateValue > maxDateTime)
            //        return new ValidationResult(string.Format("'{0}'必须在{1}与{2}之间。", validationContext.DisplayName, minDateTime, maxDateTime));
            //}

            return null;
        }
        private string FormatDate(DateTime d)
        {
            switch (_type)
            {

                case DateTimeRangeType.Day:
                    {
                        return d.Date.ToShortDateString();
                    }
                case DateTimeRangeType.Month:
                    {
                        return d.Date.ToShortDateString();
                    }

                case DateTimeRangeType.Year:
                    {
                        return d.Date.ToShortDateString();
                    }
                default:
                    return d.ToString();
            }
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
        /// <param name="diff"></param>
        /// <param name="fieldTime"></param>
        /// <returns></returns>
        private DateTime GetDateTime( DateTime fieldTime, int diff)
        {

            switch (_type)
            {
                case DateTimeRangeType.Minute:
                    {
                        return fieldTime.AddMinutes(diff);
                    }
                case DateTimeRangeType.Day:
                    {
                        return fieldTime.Date.AddDays(diff).Date;
                    }
                case DateTimeRangeType.Month:
                    {
                        return fieldTime.Date.AddMonths(diff).Date;
                    }

                case DateTimeRangeType.Year:
                    {
                        return fieldTime.Date.AddYears(diff).Date;
                    }

            }
            return fieldTime;
        }

    }
}
