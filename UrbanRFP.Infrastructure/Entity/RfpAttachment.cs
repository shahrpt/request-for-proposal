using System;

namespace UrbanRFP.Infrastructure.Entity
{
    /// <summary>
    /// Created on  :Tuesday, September 3, 2019
    /// Created by  : Waheed Iqbal
    /// Updated on  : Tuesday, September 3, 2019
    /// Updated by  : Waheed Iqbal
    /// Description : Declare datamodel object for  rfp attachment
    /// </summary>
    public class RfpAttachment
    {
        public Guid rfa_key { get; set; }
        public Guid? rfa_rfp_key { get; set; }
        public Guid? rfa_rfr_key { get; set; }
        public Guid? rfa_cxr_key { get; set; }
        public Guid? rfa_prd_key { get; set; }
        public Guid? rfa_org_key { get; set; }
        public string rfa_file_name { get; set; }
        public string rfa_file_path { get; set; }
        public string rfa_type { get; set; }
        public string rfa_mime { get; set; }
        public string rfa_attachment { get; set; }
        public string rfa_delete_flag { get; set; }
        public string rfa_add_user { get; set; }
        public DateTime rfa_add_date { get; set; }
        public string rfa_change_user { get; set; }
        public DateTime rfa_change_date { get; set; }
    }
}
