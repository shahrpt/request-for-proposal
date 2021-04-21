using System;
using System.Collections.Generic;

namespace UrbanRFP.Infrastructure.Entity
{
    public class RfpProduct
    {
        public RfpProduct()
        {
            RACIContactList = new List<ContactRACI>();
        }

        public List<ContactRACI> RACIContactList { get; set; }
        public Guid prd_key { get; set; }
        public Guid prd_org_key { get; set; }
        public string prd_name { get; set; }
        public int? prd_type { get; set; }
        public int? prd_subtype { get; set; }
        public string prd_type_name { get; set; }
        public string prd_subtype_name { get; set; }
        public string prd_description { get; set; }
        public string prd_benefits { get; set; }
        public string prd_use_cases { get; set; }
        public string prd_pricing { get; set; }
        public string prd_features { get; set; }
        public string prd_website { get; set; }
        public string prd_target_users { get; set; }
        public byte prd_delete_flag { get; set; }
        public DateTime prd_add_date { get; set; }
        public string prd_add_user { get; set; }
        public DateTime prd_change_date { get; set; }
        public string prd_change_user { get; set; }
        public int last_month_views { get; set; }
        public int avg_month_views { get; set; }
        public int total_likes { get; set; }
        public int total_views { get; set; }
        public int total_rfq { get; set; }
    }
}
