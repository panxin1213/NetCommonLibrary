using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using ChinaBM.Common;

namespace System
{
    /// <summary>
    /// ToList时返回的元素
    /// </summary>
    public struct EnumField
    {
        /// <summary>
        /// Enum的数字值
        /// </summary>
        public long EnumValue { get; set; }
        /// <summary>
        /// Enum的字符
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Enum 扩展
    /// </summary>
    public static class EnumEx
    {

        /// <summary>
        /// 获取Enum某个属性的说明 System.ComponentModel.DescriptionAttribute
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum e)
        {
            if (e == null)
                return "[未知]";
            var value = e.ToString();
            var dis = e.GetType().GetField(value).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return dis == null ? value : dis.Description;
        }
        /// <summary>
        /// 转换Enum为List包括说明
        /// </summary>
        /// <typeparam name="TEnumType"></typeparam>
        /// <returns></returns>
        public static IList<EnumField> ToList<TEnumType>() where TEnumType : struct
        {
            return typeof(TEnumType).ToList();
        }

        /// <summary>
        /// Enum Type 转List
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<EnumField> ToList(this Type type)
        {
            if (!type.IsEnum)
            {
                throw new NotSupportedException("类型应为Enum");
            }

            var typeofDescriptionAttribute = typeof(DescriptionAttribute);

            var list = type.GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(a => !a.Name.Equals("Unknown", StringComparison.OrdinalIgnoreCase))
                .Select(a =>
                {
                    var des = a.GetCustomAttributes(typeofDescriptionAttribute, false).FirstOrDefault() as DescriptionAttribute;

                    return new EnumField
                    {
                        Value = des == null ? a.Name : des.Description,
                        Key = a.Name,
                        EnumValue = Convert.ToInt64(a.GetRawConstantValue())
                    };
                }).OrderBy(a => a.EnumValue).ToList();


            return list;
        }

        public static string GetEnumValue(this Type type, long nums)
        {
            if (!type.IsEnum)
            {
                throw new NotSupportedException("类型应为Enum");
            }

            return type.ToList().SingleOrDefault(a => a.EnumValue == nums).Value;
        }

        /// <summary>
        /// 得到Enum的说明
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDescription(this Type type, string key)
        {
            if (!type.IsEnum)
            {
                throw new NotSupportedException("类型应为Enum");
            }
            if (String.IsNullOrEmpty(key))
                return "--";
            var field = type.GetField(key);
            if (field == null)
                return "--";
            var dis = field.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return dis == null ? key : dis.Description;
        }
        /// <summary>
        /// Enum泛型转换
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDescription<TEnum>(string key) where TEnum : struct
        {
            return typeof(TEnum).GetDescription(key);
        }
        /// <summary>
        /// Enum泛型转换
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TEnum Parse<TEnum>(string key) where TEnum : struct
        {
            TEnum r = default(TEnum);
            Enum.TryParse<TEnum>(key, out r);
            return r;
        }
        public static int ToInt(this Enum e)
        {
            return Int32.Parse(e.ToString("D"));
        }



        /// <summary>
        /// 得到Enum的说明
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetEnumValue(Type type, string key)
        {
            if (!type.IsEnum)
            {
                throw new NotSupportedException("类型应为Enum");
            }
            var l = ToList(type);

            foreach (var item in l)
            {
                if (item.Key == key)
                {
                    return ConvertKit.Convert(item.EnumValue, -1);
                }
            }

            return -1;
        }

    }




}
