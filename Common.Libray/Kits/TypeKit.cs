using System;

namespace ChinaBM.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeKit
    {
        /// <summary>
        /// 判断泛型类型能否从指定的类型分配
        /// </summary>
        /// <param name="givenType"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }
    }
}
