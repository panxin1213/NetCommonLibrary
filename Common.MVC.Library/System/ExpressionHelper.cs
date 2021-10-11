using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc;
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
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="methoName"></param>
        /// <returns></returns>
        public static EnumDataType GetEnumDataFromLambdaExpression<TModel, TValue>(HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string methoName)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var j = metadata.Properties;
            var type = metadata.ContainerType;

            var typeDescriptor = new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
            PropertyDescriptor property = typeDescriptor.GetProperties().Find(metadata.PropertyName, true);
            var xx = property.Attributes.Cast<Attribute>().Where(a => a is EnumDataTypeAttribute).LastOrDefault() as EnumDataTypeAttribute;
            if (xx == null)
            {
                throw new Exception(String.Format("方法“Html.{0}({1})”出错， 请在“{2}”的 MetaData 中相应字段“{3}”添加 “EnumDataTypeAttribute” 属性", methoName, expression.ToString(), type.FullName, metadata.PropertyName));
            }
            return new EnumDataType
            {
                Value = metadata.SimpleDisplayText,
                EnumType = xx.EnumType
            };
            //return MvcHtmlString.Create(xx.EnumType.GetDescription(metadata.SimpleDisplayText));
        }
#endregion

        public static Dictionary<string, string> GetOptionParamDataFromLambdaExpression<TModel, TValue>(HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string methodName)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var j = metadata.Properties;
            var type = metadata.ContainerType;

            var typeDescriptor = new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
            PropertyDescriptor property = typeDescriptor.GetProperties().Find(metadata.PropertyName, true);
            var xx = property.Attributes.Cast<Attribute>().Where(a => a is OptionParamAttribute).LastOrDefault() as OptionParamAttribute;
            if (xx == null)
            {
                throw new Exception(String.Format("方法“Html.{0}({1})”出错， 请在“{2}”的相应字段“{3}”添加 “OptionParamAttribute” 属性,并在web.config中的appSettings中添加“{3}”文件路径配置", methodName, expression.ToString(), type.FullName, metadata.PropertyName));
            }

            return xx.Params;
        }


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

    }
}
