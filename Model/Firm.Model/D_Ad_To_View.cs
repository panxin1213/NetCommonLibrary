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
    [Dapper.Contrib.Extensions.Table("D_Ad_To_View")]
    public partial class D_Ad_To_View
    {

    [Dapper.Contrib.Extensions.KeyAttribute]
    	[Required]
        public int F_M_To_V_View_ID { get; set; }

    [Dapper.Contrib.Extensions.KeyAttribute]
    	[Required]
        public int F_M_To_V_AD_ID { get; set; }

    	[Required]
        public System.DateTime F_M_To_V_EndTime { get; set; }

    	[Required]
        public System.DateTime F_M_To_V_StartTime { get; set; }

    	[Required]
        public int F_M_To_V_Order { get; set; }
    	
    	
    [Dapper.Contrib.Extensions.Write(false)]
    [NotMapped]
        public virtual D_Ad D_Ad { get; set; }
    [Dapper.Contrib.Extensions.Write(false)]
    [NotMapped]
        public virtual D_Ad_View D_Ad_View { get; set; }
    }
    
}