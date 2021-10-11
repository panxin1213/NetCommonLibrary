using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Firm.Model
{
    [Dapper.Contrib.Extensions.Table("D_Admin_To_Role")]
    public class D_Admin_To_Role
    {
        [Dapper.Contrib.Extensions.KeyAttribute]
        [Required]
        public int F_Admin_Role_AdminId { get; set; }

        [Dapper.Contrib.Extensions.KeyAttribute]
        [Required]
        public int F_Admin_Role_RoleId { get; set; }

        [Dapper.Contrib.Extensions.Write(false)]
        [NotMapped]
        public virtual D_Admin_Role D_Admin_Role { get; set; }

        [Dapper.Contrib.Extensions.Write(false)]
        [NotMapped]
        public virtual D_Admin D_Admin { get; set; }
    }
}
