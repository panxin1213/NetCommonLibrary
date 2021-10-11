using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace System.Linq
{
    #region enum相关
    /// <summary>
    /// Enum类型
    /// </summary>
    public struct EnumDataType
    {
        /// <summary>
        /// 值
        /// </summary>
        public string Value;
        /// <summary>
        /// type
        /// </summary>
        public Type EnumType;
    }
    /// <summary>
    /// 表达式扩展
    /// </summary>
    public static class ExpressionEx
    {
#endregion


        /// <summary>
        /// 表达式中的属性名
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="property"></param>
        /// <param name="methodName"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string ParsePropertySelector<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> property, string methodName, string paramName)
        {
            string str;
            if (!TryParsePath(property.Body, out str) || (str == null))
            {
                //DbEntityEntry_BadPropertyExpression=The expression passed to method {0} must represent a property defined on the type '{1}'.
                throw new ArgumentException(string.Format("传递给方法{0}的表达必须是类型“{1}”的一个属性 ，参数：“{2}”", methodName, typeof(TEntity).Name, paramName));
            }
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool TryParsePath(Expression expression, out string path)
        {
            path = null;
            Expression expression2 = expression.RemoveConvert();
            MemberExpression expression3 = expression2 as MemberExpression;
            MethodCallExpression expression4 = expression2 as MethodCallExpression;
            if (expression3 != null)
            {
                string str2;
                string name = expression3.Member.Name;
                if (!TryParsePath(expression3.Expression, out str2))
                {
                    return false;
                }
                path = (str2 == null) ? name : (str2 + "." + name);
            }
            else if (expression4 != null)
            {
                if ((expression4.Method.Name == "Select") && (expression4.Arguments.Count == 2))
                {
                    string str3;
                    if (!TryParsePath(expression4.Arguments[0], out str3))
                    {
                        return false;
                    }
                    if (str3 != null)
                    {
                        LambdaExpression expression5 = expression4.Arguments[1] as LambdaExpression;
                        if (expression5 != null)
                        {
                            string str4;
                            if (!TryParsePath(expression5.Body, out str4))
                            {
                                return false;
                            }
                            if (str4 != null)
                            {
                                path = str3 + "." + str4;
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Expression RemoveConvert(this Expression expression)
        {
            while ((expression != null) && ((expression.NodeType == ExpressionType.Convert) || (expression.NodeType == ExpressionType.ConvertChecked)))
            {
                expression = ((UnaryExpression)expression).Operand.RemoveConvert();
            }
            return expression;
        }
    }
}
