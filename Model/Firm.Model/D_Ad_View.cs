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
    [Dapper.Contrib.Extensions.Table("D_Ad_View")]
    public partial class D_Ad_View
    {
        public D_Ad_View()
        {
            this.D_Ad_To_View = new List<D_Ad_To_View>();
    		var type = typeof(D_Ad_View);
    		var method = type.GetMethod("PartialStructure", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method != null)
            {
                method.Invoke(this, null);
            }
        }
    

    [Dapper.Contrib.Extensions.KeyAttribute]
    	[Required]
        public int F_View_Id { get; set; }

    	[Required]
        [StringLength(50)]
        public string F_View_Title { get; set; }

    	[Required]
        [StringLength(500)]
        public string F_View_Address { get; set; }

        public Nullable<System.DateTime> F_View_Create { get; set; }
    	
    	
    [Dapper.Contrib.Extensions.Write(false)]
    [NotMapped]
        public virtual IList<D_Ad_To_View> D_Ad_To_View { get; set; }
    }
    
}
