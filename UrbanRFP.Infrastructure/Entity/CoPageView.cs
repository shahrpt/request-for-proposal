using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanRFP.Infrastructure.Entity
{
    public class CoPageView
    {
        public Guid pv_key { get; set; }
        public Guid? pv_rfp_key { get; set; }
        public Guid? pv_prd_key { get; set; }
        public Guid? pv_source_ct_key { get; set; }
        public int? pv_page_id { get; set; }
        public DateTime? pv_view_datetime { get; set; }
        public string pv_add_user { get; set; }
    }
}
