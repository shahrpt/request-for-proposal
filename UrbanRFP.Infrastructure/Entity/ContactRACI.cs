using System;

namespace UrbanRFP.Infrastructure.Entity
{
    public class ContactRACI
    {
        public Guid ct_key { get; set; }
        public long? ct_id { get; set; }
        public string ct_first_name { get; set; }
        public string ct_last_name { get; set; }
        public string ct_email { get; set; }
        public string ct_phone { get; set; }
        public string ct_fax { get; set; }
        public string ct_password { get; set; }
        public byte? ct_delete_flag { get; set; }
        public string ct_add_date { get; set; }
        public string ct_add_user { get; set; }
        public string ct_change_date { get; set; }
        public string ct_change_user { get; set; }
        public Guid ct_activation_code { get; set; }
        public Int16 ct_active { get; set; }
        public string cxo_rlt_code { get; set; }
        public DateTime? cxo_start_date { get; set; }
        public string cxo_title { get; set; }
        public string org_legal_name { get; set; }
        public string lk5_code { get; set; }
        public Guid? lk5_key { get; set; }
        public string lk5_value { get; set; }
    }
}
