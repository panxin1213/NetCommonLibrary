//------------------------------------------------------------------------------
// <auto-generated>
//		自动生成，请勿修改 
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace Firm.Model
{
    [Dapper.Contrib.Extensions.Table("D_Admin_Role_Right")]
    public partial class D_Admin_Role_Right
    {

    [Dapper.Contrib.Extensions.KeyAttribute]
    	[Required]
        public int F_Admin_Right_RoleId { get; set; }

    [Dapper.Contrib.Extensions.KeyAttribute]
    	[Required]
        [StringLength(50)]
        public string NameSpace { get; set; }

    [Dapper.Contrib.Extensions.KeyAttribute]
    	[Required]
        [StringLength(50)]
        public string ClassName { get; set; }
    	
    	
    [Dapper.Contrib.Extensions.Write(false)]
    [NotMapped]
        public virtual D_Admin_Role D_Admin_Role { get; set; }
    }
    
}