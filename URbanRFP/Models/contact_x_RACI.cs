//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ICMA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class contact_x_RACI
    {
        public System.Guid rxc_key { get; set; }
        public Nullable<System.Guid> rxc_lk5_key { get; set; }
        public Nullable<System.Guid> rxc_ct_key { get; set; }
        public Nullable<System.Guid> rxc_rfp_key { get; set; }
        public Nullable<System.Guid> rxc_prd_key { get; set; }
        public Nullable<byte> rxc_delete_flag { get; set; }
        public Nullable<System.DateTime> rxc_add_date { get; set; }
        public string rxc_add_user { get; set; }
        public Nullable<System.DateTime> rxc_change_date { get; set; }
        public string rxc_change_user { get; set; }
    
        public virtual co_contact co_contact { get; set; }
        public virtual lkup_RACI lkup_RACI { get; set; }
        public virtual rfp_product rfp_product { get; set; }
        public virtual rfp_request rfp_request { get; set; }
    }
}
