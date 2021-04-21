using System;

namespace UrbanRFP.Infrastructure.Entity
{
    public class RfpRespondInvitation
    {
        public Guid? inv_key { get; set; }
        public Guid? inv_rfp_key { get; set; }
        public Guid? inv_ct_key { get; set; }
        public string inv_first_name { get; set; }
        public string inv_last_name { get; set; }
        public string inv_full_name { get; set; }
        public string inv_email { get; set; }
        public string inv_status { get; set; }
        public Int16 inv_delete_flag { get; set; }
        public DateTime? inv_add_date { get; set; }
        public string inv_add_user { get; set; }
        public DateTime? inv_change_date { get; set; }
        public string inv_change_user { get; set; }
    }
}
