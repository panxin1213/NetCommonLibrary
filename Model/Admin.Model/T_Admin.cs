//------------------------------------------------------------------------------
// <auto-generated>
//		自动生成，请勿修改 
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
namespace Admin.Model
{
    [Serializable]
    [Dapper.Contrib.Extensions.Table("T_Admin")]
    public partial class T_Admin
    {
    	
        public T_Admin()
        {
            this.T_Admin_Log = new List<T_Admin_Log>();
            this.T_Admin_Role = new List<T_Admin_Role>();
    		var type = typeof(T_Admin);
    		var method = type.GetMethod("PartialStructure", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method != null)
            {
                method.Invoke(this, null);
            }
        }
    

    [Dapper.Contrib.Extensions.KeyAttribute]
    	[Required]
        public int F_Admin_Id { get; set; }

    	[Required]
        [StringLength(50)]
        public string F_Admin_Name { get; set; }

        [StringLength(50)]
        public string F_Admin_RealName { get; set; }

    	[Required]
        [StringLengthChar(32)]
        public string F_Admin_Password { get; set; }

    	[Required]
        public System.DateTime F_Admin_Create { get; set; }

    	[Required]
        public bool F_Admin_IsLock { get; set; }

    	[Required]
        public bool F_Admin_IsSupper { get; set; }
    	
    	
    [Dapper.Contrib.Extensions.Write(false)]
    [NotMapped]
        public virtual IList<T_Admin_Log> T_Admin_Log { get; set; }
    [Dapper.Contrib.Extensions.Write(false)]
    [NotMapped]
        public virtual IList<T_Admin_Role> T_Admin_Role { get; set; }
    }
    
}
