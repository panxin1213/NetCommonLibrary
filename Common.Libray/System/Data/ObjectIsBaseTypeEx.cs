using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// object扩展
    /// </summary>
    public static class ObjectIsBaseTypeEx
    {
        private static Dictionary<Type, bool> typeMap = new Dictionary<Type, bool>()
        {
            {typeof(byte),true},
            {typeof(sbyte),true},
            {typeof(short),true},
            {typeof(ushort),true},
            {typeof(int),true},
            {typeof(uint),true},
            {typeof(long),true},
            {typeof(ulong),true},
            {typeof(float),true},
            {typeof(double),true},
            {typeof(decimal),true},
            {typeof(bool),true},
            {typeof(string),true},
            {typeof(char),true},
            {typeof(Guid),true},
            {typeof(DateTime),true},
            {typeof(DateTimeOffset),true},
            {typeof(byte[]),true},
            {typeof(byte?),true},
            {typeof(sbyte?),true},
            {typeof(short?),true},
            {typeof(ushort?),true},
            {typeof(int?),true},
            {typeof(uint?),true},
            {typeof(long?),true},
            {typeof(ulong?),true},
            {typeof(float?),true},
            {typeof(double?),true},
            {typeof(decimal?),true},
            {typeof(bool?),true},
            {typeof(char?),true},
            {typeof(Guid?),true},
            {typeof(DateTime?),true},
            {typeof(DateTimeOffset?),true}

        };

        /// <summary>
        /// 是否是基础类
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsBaseType(this object o)
        {
            if (o == null)
            {
                return false;
            }

            if (o is Type)
            {
                return typeMap.ContainsKey(o as Type);
            }

            return typeMap.ContainsKey(o.GetType());


        }
    }
}
