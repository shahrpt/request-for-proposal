using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanRFP.Domain.ViewModel
{
    public class RfpOrgViewModel
    {
        public Guid rfp_key { get; set; }
        public Guid rfp_org_key { get; set; }
        public int rfp_id { get; set; }
        public string rfp_name { get; set; }
        public string rfp_summary { get; set; }
        public string rfp_background { get; set; }
        public string rfp_bid_number_in_LG { get; set; }
        public DateTime? rfp_pre_proposal_meeting_date { get; set; }
        public string rfp_pre_proposal_meeting_location { get; set; }
        public string rfp_submission_instructions { get; set; }
        public int rfp_type { get; set; }
        public int rfp_subtype { get; set; }
        public string rfp_type_name { get; set; }
        public string rfp_subtype_name { get; set; }
        public string rfp_solo_multiple_sources { get; set; }
        public DateTime? rfp_issue_date { get; set; }
        public DateTime? rfp_question_deadline { get; set; }
        public DateTime? rfp_question_answer_deadline { get; set; }
        public DateTime? rfp_vendor_meeting_date { get; set; }
        public DateTime? rfp_close_date { get; set; }
        public DateTime? rfp_scoring_date { get; set; }
        public DateTime? rfp_vendor_post_submission_meeting_date { get; set; }
        public DateTime? rfp_decision_date { get; set; }
        public DateTime? rfp_award_date { get; set; }
        public DateTime? rfp_validation_date { get; set; }
        public string rfp_scope_of_work { get; set; }
        public string rfp_sow_introduction { get; set; }
        public string rfp_sow_solution_requirements { get; set; }
        public string rfp_sow_project_timeline { get; set; }
        public string rfp_sow_cost { get; set; }
        public string rfp_skills { get; set; }
        public decimal rfp_budget { get; set; }
        public decimal rfp_awarded { get; set; }
        public string rfp_department { get; set; }
        public decimal rfp_view_count { get; set; }
        public byte rfp_delete_flag { get; set; }
        public DateTime? rfp_add_date { get; set; }
        public string rfp_add_user { get; set; }
        public DateTime? rfp_change_date { get; set; }
        public string rfp_change_user { get; set; }
        public string rfp_questionnaire { get; set; }
        public string rfp_evaluation_process { get; set; }
        public string rfp_evaluation_criteria { get; set; }
        public string rfp_terms_conditions { get; set; }
        public string rfp_date_notes { get; set; }
        public string org_legal_name { get; set; }
        public string org_description { get; set; }
        public int favorite { get; set; }
        public bool rfp_published { get; set; }
        public int response_count { get; set; }
        public int score_count { get; set; }
        public bool scoring_completed { get; set; }
        public string actions { get; set; }
        public int last_month_views { get; set; }
        public int avg_month_views { get; set; }
        public int total_likes { get; set; }
        public int total_views { get; set; }
    }
}
