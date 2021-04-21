using System;

namespace UrbanRFP.Infrastructure.Entity
{
    public class ProductRfpType
    {
        public Guid prc_key { get; set; }
        public Guid prc_rfp_key { get; set; }
        public Guid prc_prd_key { get; set; }
        public int prc_type_id { get; set; }
        public string prc_type_name { get; set; }
        public int? prc_subtype_id { get; set; }
        public string prc_subtype_name { get; set; }
        public Int16 prc_delete_flag { get; set; }
        public DateTime prc_add_date { get; set; }
        public string prc_add_user { get; set; }
        public DateTime? prc_change_date { get; set; }
        public string prc_change_user { get; set; }
    }
}
