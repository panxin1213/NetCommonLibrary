namespace Firm.Core.Admin.ManagePermissions
{
    using System;
    using System.Security.Principal;
    using System.Web;

    [Serializable]
    public class Identity : IIdentity
    {
        private readonly DateTime _createTime;
        public DateTime CreateTime
        {
            get { return this._createTime; }
        }

        private DateTime _lastActiveTime;
        public DateTime LastActiveTime
        {
            get { return this._lastActiveTime; }
            set { this._lastActiveTime = value; }
        }

        private readonly int _id;
        public int Id
        {
            get { return this._id; }
        }
        private bool _isSupper = false;
        public bool IsSupper
        {
            get
            {
                return _isSupper;
            }
        }
        public Identity()
        {
            this._createTime = DateTime.Now;
            this.LastActiveTime = DateTime.Now;
            this._nick_name = "";
            this._userName = "未登录";
            this._id = 0;
        }

        public Identity(int id, string username, string name, bool isSupper, bool isAuthenticated)
        {
            this._createTime = DateTime.Now;
            this.LastActiveTime = DateTime.Now;
            this._nick_name = name;
            this._userName = username;
            this._id = id;
            this._isSupper = isSupper;
            this._isAuthenticated = isAuthenticated;
        }

        private string _userName = null;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                this._userName = value;
            }
        }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool _isAuthenticated = false;

        public bool IsAuthenticated
        {
            get
            {
                return _isAuthenticated;
            }
        }

        private string _nick_name = null;
        public string Name
        {
            get
            {
                return _nick_name;
            }
            set
            {
                this._nick_name = value;
            }
        }
    }
}
