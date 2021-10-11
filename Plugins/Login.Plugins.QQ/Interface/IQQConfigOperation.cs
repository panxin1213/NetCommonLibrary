using Login.Plugins.QQ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Plugins.QQ.Interface
{
    /// <summary>
    /// 获取qq互联配置接口
    /// </summary>
    public interface IQQConfigOperation
    {
        /// <summary>
        /// 获取本地存储的access_token
        /// </summary>
        /// <returns></returns>
        QQAccessTokenModel GetAccessToken();

        /// <summary>
        /// 授权成功后操作方法
        /// </summary>
        /// <param name="openid"></param>
        void AuthSuccessCallBack(string openid, QQAccessTokenModel tokenmodel);


        void ReturnErrorPage(string error);
    }
}
