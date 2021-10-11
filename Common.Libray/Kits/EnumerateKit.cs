namespace ChinaBM.Common
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.ComponentModel;

    public static class EnumerateKit
    {
        public static IDictionary<string, string> ToDescriptionDictionary(Type enumType)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
          
            object enumInstance = enumType.Assembly.CreateInstance(enumType.FullName);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (descriptionAttributes.Length > 0)
                {
                    dictionary.Add(Convert.ToSingle((byte)fieldInfo.GetValue(enumInstance)).ToString(), descriptionAttributes[0].Description);
                }
            }
            return dictionary;
        }
    }
}
