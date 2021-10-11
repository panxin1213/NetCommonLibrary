using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Login.Plugins.QQ.Model
{
    public class OpenIdModel
    {
        public OpenIdModel(string s)
        {
            var d = s.ToSafeString().Replace("callback(", "").Replace(");\n", "").JsonToDictionary();

            ClientId = d.TryGetValue("client_id", "").ToSafeString();
            OpenId = d.TryGetValue("openid", "").ToSafeString();
        }

        public string ClientId { get; set; }

        public string OpenId { get; set; }
    }
}
