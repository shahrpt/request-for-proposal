using System;
using System.Collections.Generic;
using System.Text;

namespace UrbanRFP.Domain.ViewModel
{
    public class ProductOrganizationViewModel
    {
        public Guid prd_key { get; set; }
        public string prd_name { get; set; }
        public string prd_type { get; set; }
        public string prd_subtype { get; set; }
        public int prd_type_id { get; set; }
        public int prd_subtype_id { get; set; }
        public string prd_description { get; set; }
        public string prd_benefits { get; set; }
        public Guid org_key { get; set; }
        public string org_legal_name { get; set; }
        public string prd_features { get; set; }
        public string prd_pricing { get; set; }
        public string prd_target_users { get; set; }
        public int org_id { get; set; }
        public string org_description { get; set; }
        public string prd_website { get; set; }
        public string org_specialities { get; set; }
        public byte org_qualified_LG_flag { get; set; }
        public string org_federal_tax_id { get; set; }
        public decimal org_ann_revenue { get; set; }
        public int org_vendor_number_of_employees { get; set; }
        public int favorite { get; set; }
        public int last_month_views { get; set; }
        public int? avg_month_views { get; set; }
        public int invites { get; set; }
        public string actions { get; set; }
        public string prd_type_name { get; set; }
        public string prd_subtype_name { get; set; }
        public int total_likes { get; set; }
        public int total_views { get; set; }
        public int total_rfq { get; set; }
    }
}
