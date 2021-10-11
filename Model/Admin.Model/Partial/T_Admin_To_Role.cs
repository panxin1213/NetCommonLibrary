using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Admin.Model
{
    [Dapper.Contrib.Extensions.Table("T_Admin_To_Role")]
    public class T_Admin_To_Role
    {
        [Dapper.Contrib.Extensions.KeyAttribute]
        [Required]
        public int F_Admin_Role_AdminId { get; set; }

        [Dapper.Contrib.Extensions.KeyAttribute]
        [Required]
        public int F_Admin_Role_RoleId { get; set; }
    }
}
