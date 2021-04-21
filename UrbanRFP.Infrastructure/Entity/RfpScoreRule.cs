using System;

namespace UrbanRFP.Infrastructure.Entity
{
    public class RfpScoreRule
    {
        public Guid? eva_key { get; set; }
        public Guid? eva_rfs_key { get; set; }
        public Guid? eva_rfp_key { get; set; }
        public string eva_criteria_name { get; set; }
        public string eva_criteria_description { get; set; }
        public string eva_score_method { get; set; }
        public int? eva_point_max { get; set; }
        public string eva_weight { get; set; }
        public string eva_delete_flag { get; set; }
        public string eva_add_date { get; set; }
        public string eva_add_user { get; set; }
        public string eva_change_date { get; set; }
        public string eva_change_user { get; set; }
    }
}
