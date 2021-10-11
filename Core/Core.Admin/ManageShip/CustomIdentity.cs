using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace System.Web
{
    /// <summary>
    /// 用户基本数据存入Session
    /// </summary>
    [Serializable]
    public class CustomIdentity : IIdentity
    {
        private static readonly string DEFAULT_USER_NAME = "Guest";
        private static readonly string DEFAULT_NICK_NAME = "匿名网友";


        private string _name = DEFAULT_USER_NAME;
        private string _nick_name = DEFAULT_NICK_NAME;
        private int _id = 0;
        private bool _isAuthenticated = false;
        private bool _isSupper = false;


        /// <summary>
        /// 是否未登录的匿名用户
        /// </summary>
        /// <returns></returns>
        public bool IsDefaultUser()
        {
            return DEFAULT_USER_NAME.Equals(_name) && DEFAULT_NICK_NAME.Equals(_nick_name) && _id == 0;
        }

        /// <summary>
        /// 无参数的构造函数初始化一个匿名用户 IsAuthenticated = false 
        /// </summary>
        public CustomIdentity()
        {

        }
        /// <summary>
        /// 初始化一个可能登录的用户 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="nick"></param>
        public CustomIdentity(int id, string username, string nick_name, bool isSupper, bool isAuthenticated)
        {
            _name = username ?? DEFAULT_USER_NAME;
            _nick_name = nick_name ?? DEFAULT_NICK_NAME;
            _id = id;
            _isSupper = isSupper;
            _isAuthenticated = isAuthenticated;
        }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
        }

        public string UserName
        {
            get
            {
                return _name;
            }
        }
        public int ID
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get { return _nick_name; }
        }


        public bool IsSupper
        {
            get
            {
                return _isSupper;
            }
        }
    }
}