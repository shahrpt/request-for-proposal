using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UrbanRFP.Infrastructure.Entity
{
    public class RfpRequest
    {
        public RfpRequest()
        {
            ContactList = new List<string>();
            RACIContactList = new List<ContactRACI>();
            ScoreRules = new List<RfpScoreRule>();
        }

        public Guid rfp_key { get; set; }
        public Guid rfp_org_key { get; set; }
        public Guid? rfp_prd_key { get; set; }
        public int rfp_id { get; set; }

        [Required]
        [Display(Description = "Title")]
        public string rfp_name { get; set; }
        [AllowHtml]
        public string rfp_summary { get; set; }
        public string rfp_background { get; set; }
        public string rfp_bid_number_in_LG { get; set; }
        public DateTime? rfp_pre_proposal_meeting_date { get; set; }
        public string rfp_pre_proposal_meeting_location { get; set; }
        [AllowHtml]
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
        [AllowHtml]
        public string rfp_scope_of_work { get; set; }
        [AllowHtml]
        public string rfp_sow_introduction { get; set; }
        [AllowHtml]
        public string rfp_sow_solution_requirements { get; set; }
        [AllowHtml]
        public string rfp_sow_project_timeline { get; set; }
        [AllowHtml]
        public string rfp_sow_cost { get; set; }
        [AllowHtml]
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
        [AllowHtml]
        public string rfp_questionnaire { get; set; }
        [AllowHtml]
        public string rfp_evaluation_process { get; set; }
        [AllowHtml]
        public string rfp_evaluation_criteria { get; set; }
        [AllowHtml]
        public string rfp_terms_conditions { get; set; }
        [AllowHtml]
        public string rfp_date_notes { get; set; }
        public string RfpAttachments { get; set; }
        public string QuestionnaireFiles { get; set; }
        public List<string> ContactList { get; set; }
        public List<ContactRACI> RACIContactList { get; set; }
        public List<RfpScoreRule> ScoreRules { get; set; }
        public bool rfp_published { get; set; }
        public bool scoring_completed { get; set; }
        public int rfp_progress { get; set; }
        public int last_month_views { get; set; }
        public int avg_month_views { get; set; }
        public int total_likes { get; set; }
        public int total_views { get; set; }
    }
}
