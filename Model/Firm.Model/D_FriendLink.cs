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
    [Dapper.Contrib.Extensions.Table("D_FriendLink")]
    public partial class D_FriendLink
    {

    [Dapper.Contrib.Extensions.KeyAttribute]
    	[Required]
        public int F_FriendLink_Id { get; set; }

    	[Required]
        [StringLength(50)]
        public string F_FriendLink_Title { get; set; }

    	[Required]
        [StringLength(500)]
        public string F_FriendLink_Url { get; set; }

    	[Required]
        public byte F_FriendLink_Type { get; set; }

        [StringLength(500)]
        public string F_FriendLink_Image { get; set; }

    	[Required]
        public bool F_FriendLink_IsLock { get; set; }

        public Nullable<System.DateTime> F_FriendLink_Create { get; set; }

        public Nullable<System.DateTime> F_FriendLink_Update { get; set; }

    	[Required]
        public int F_FriendLink_Order { get; set; }

        [StringLength(200)]
        public string F_FriendLink_ByAddress { get; set; }
    	
    	}
    
}
