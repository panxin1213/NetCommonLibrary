using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Model
{
    public partial class T_Admin_Role_Right
    {
        [Dapper.Contrib.Extensions.Write(false)]
        public bool Checked { get; set; }
    }
}
