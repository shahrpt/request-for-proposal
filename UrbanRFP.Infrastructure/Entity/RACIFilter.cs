using System;

namespace UrbanRFP.Infrastructure.Entity
{
    public class RACIFilter
    {
        public RACIFilter()
        {
            rfp_key = string.Empty;
            prd_key = string.Empty;
        }

        public Guid Org_key { get; set; }
        public string rfp_key { get; set; }
        public string prd_key { get; set; }
    }
}
