using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISearchCondition
    {
        /// <summary>
        /// 根据搜索模型里的字段生成sql
        /// </summary>
        /// <returns></returns>
        string GetSql();
        /// <summary>
        /// 根据搜索模型里的字段 生成页面title
        /// </summary>
        /// <returns></returns>
        String GetSearchTitle();

        /// <summary>
        /// 根据搜索模型里的字段 生成页面MetaKey
        /// </summary>
        /// <returns></returns>
        String GetSearchMetaKey();

        /// <summary>
        /// 根据搜索模型里的字段 生成页面MetaContent
        /// </summary>
        /// <returns></returns>
        String GetSearchMetaContent();

        /// <summary>
        /// 获取搜索主关键词List(key{pro,主关键字前}{next,主关键字后})
        /// </summary>
        /// <returns></returns>
        Dictionary<string, List<string>> GetSearchKeyList();
    }
}
