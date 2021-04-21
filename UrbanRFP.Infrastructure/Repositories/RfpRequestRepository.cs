using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Infrastructure.Repositories
{
    public class RfpRequestRepository : IRfpRequestRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public RfpRequestRepository(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public Response<RfpRequest> Add(RfpRequest item, string user)
        {
            try
            {
                item.rfp_key = Guid.NewGuid();
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@rfp_key", item.rfp_key, DbType.Guid);
                _params.Add("@rfp_org_key", item.rfp_org_key, DbType.Guid);
                _params.Add("@rfp_prd_key", item.rfp_prd_key, DbType.Guid);
                _params.Add("@rfp_id", item.rfp_id, DbType.Int64);
                _params.Add("@rfp_name", item.rfp_name, DbType.String);
                _params.Add("@rfp_summary", item.rfp_summary, DbType.String);
                _params.Add("@rfp_background", item.rfp_background, DbType.String);
                _params.Add("@rfp_bid_number_in_LG", item.rfp_bid_number_in_LG, DbType.String);
                _params.Add("@rfp_pre_proposal_meeting_date", item.rfp_pre_proposal_meeting_date, DbType.DateTime);
                _params.Add("@rfp_pre_proposal_meeting_location", item.rfp_pre_proposal_meeting_location, DbType.String);
                _params.Add("@rfp_submission_instructions", item.rfp_submission_instructions, DbType.String);
                _params.Add("@rfp_type", item.rfp_type, DbType.Int32);
                _params.Add("@rfp_subtype", item.rfp_subtype, DbType.Int32);
                _params.Add("@rfp_solo_multiple_sources", item.rfp_solo_multiple_sources, DbType.String);
                _params.Add("@rfp_issue_date", item.rfp_issue_date, DbType.DateTime);
                _params.Add("@rfp_question_deadline", item.rfp_question_deadline, DbType.DateTime);
                _params.Add("@rfp_question_answer_deadline", item.rfp_question_answer_deadline, DbType.DateTime);
                _params.Add("@rfp_vendor_meeting_date", item.rfp_vendor_meeting_date, DbType.DateTime);
                _params.Add("@rfp_close_date", item.rfp_close_date, DbType.DateTime);
                _params.Add("@rfp_scoring_date", item.rfp_scoring_date, DbType.DateTime);
                _params.Add("@rfp_vendor_post_submission_meeting_date", item.rfp_vendor_post_submission_meeting_date, DbType.DateTime);
                _params.Add("@rfp_decision_date", item.rfp_decision_date, DbType.DateTime);
                _params.Add("@rfp_award_date", item.rfp_award_date, DbType.DateTime);
                _params.Add("@rfp_validation_date", item.rfp_validation_date, DbType.DateTime);
                _params.Add("@rfp_scope_of_work", item.rfp_scope_of_work, DbType.String);
                _params.Add("@rfp_sow_introduction", item.rfp_sow_introduction, DbType.String);
                _params.Add("@rfp_sow_solution_requirements", item.rfp_sow_solution_requirements, DbType.String);
                _params.Add("@rfp_sow_project_timeline", item.rfp_sow_project_timeline, DbType.String);
                _params.Add("@rfp_sow_cost", item.rfp_sow_cost, DbType.String);
                _params.Add("@rfp_skills", item.rfp_skills, DbType.String);
                _params.Add("@rfp_budget", item.rfp_budget, DbType.Double);
                _params.Add("@rfp_awarded", item.rfp_awarded, DbType.Double);
                _params.Add("@rfp_department", item.rfp_department, DbType.String);
                _params.Add("@rfp_view_count", item.rfp_view_count, DbType.Int32);
                _params.Add("@rfp_add_user", item.rfp_add_user, DbType.String);
                _params.Add("@rfp_questionnaire", item.rfp_questionnaire, DbType.String);
                _params.Add("@rfp_evaluation_process", item.rfp_evaluation_process, DbType.String);
                _params.Add("@rfp_evaluation_criteria", item.rfp_evaluation_criteria, DbType.String);
                _params.Add("@rfp_terms_conditions", item.rfp_terms_conditions, DbType.String);
                _params.Add("@rfp_date_notes", item.rfp_date_notes, DbType.String);
                _params.Add("@rfp_published", item.rfp_published, DbType.Boolean);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_Request_Add", _params, null, null, CommandType.StoredProcedure);

                return new Response<RfpRequest>(item)
                {
                    Success = _params.Get<Int32>("success") == 1 ? true : false
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<RfpRequest> Find(string id)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@rfp_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.QuerySingle<RfpRequest>("USP_Rfp_Request_Get", _params, null, null, CommandType.StoredProcedure);

                return new Response<RfpRequest>(result)
                {
                    Success = result == null ? false : true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RfpRequest> GetAll()
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Rfp_Request_GetWhere", new
                {
                    Search = ""
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<RfpRequest>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RfpRequest> GetWhere(string search)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Rfp_Request_GetWhere", new
                {
                    Search = search
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<RfpRequest>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Remove(string id, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@rfp_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_Request_Delete", _params, null, null, CommandType.StoredProcedure);

                return _params.Get<Int32>("success") == 1 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<RfpRequest> Update(RfpRequest item, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@rfp_key", item.rfp_key.ToString(), DbType.String);
                _params.Add("@rfp_org_key", item.rfp_org_key.ToString(), DbType.String);
                _params.Add("@rfp_prd_key", item.rfp_prd_key.ToString(), DbType.String);
                _params.Add("@rfp_id", item.rfp_id, DbType.Int64);
                _params.Add("@rfp_name", item.rfp_name, DbType.String);
                _params.Add("@rfp_summary", item.rfp_summary, DbType.String);
                _params.Add("@rfp_background", item.rfp_background, DbType.String);
                _params.Add("@rfp_bid_number_in_LG", item.rfp_bid_number_in_LG, DbType.String);
                _params.Add("@rfp_pre_proposal_meeting_date", item.rfp_pre_proposal_meeting_date, DbType.DateTime);
                _params.Add("@rfp_pre_proposal_meeting_location", item.rfp_pre_proposal_meeting_location, DbType.String);
                _params.Add("@rfp_submission_instructions", item.rfp_submission_instructions, DbType.String);
                _params.Add("@rfp_type", item.rfp_type, DbType.Int32);
                _params.Add("@rfp_subtype", item.rfp_subtype, DbType.Int32);
                _params.Add("@rfp_solo_multiple_sources", item.rfp_solo_multiple_sources, DbType.String);
                _params.Add("@rfp_issue_date", item.rfp_issue_date, DbType.DateTime);
                _params.Add("@rfp_question_deadline", item.rfp_question_deadline, DbType.DateTime);
                _params.Add("@rfp_question_answer_deadline", item.rfp_question_answer_deadline, DbType.DateTime);
                _params.Add("@rfp_vendor_meeting_date", item.rfp_vendor_meeting_date, DbType.DateTime);
                _params.Add("@rfp_close_date", item.rfp_close_date, DbType.DateTime);
                _params.Add("@rfp_scoring_date", item.rfp_scoring_date, DbType.DateTime);
                _params.Add("@rfp_vendor_post_submission_meeting_date", item.rfp_vendor_post_submission_meeting_date, DbType.DateTime);
                _params.Add("@rfp_decision_date", item.rfp_decision_date, DbType.DateTime);
                _params.Add("@rfp_award_date", item.rfp_award_date, DbType.DateTime);
                _params.Add("@rfp_validation_date", item.rfp_validation_date, DbType.DateTime);
                _params.Add("@rfp_scope_of_work", item.rfp_scope_of_work, DbType.String);
                _params.Add("@rfp_sow_introduction", item.rfp_sow_introduction, DbType.String);
                _params.Add("@rfp_sow_solution_requirements", item.rfp_sow_solution_requirements, DbType.String);
                _params.Add("@rfp_sow_project_timeline", item.rfp_sow_project_timeline, DbType.String);
                _params.Add("@rfp_sow_cost", item.rfp_sow_cost, DbType.String);
                _params.Add("@rfp_skills", item.rfp_skills, DbType.String);
                _params.Add("@rfp_budget", item.rfp_budget, DbType.Double);
                _params.Add("@rfp_awarded", item.rfp_awarded, DbType.Double);
                _params.Add("@rfp_department", item.rfp_department, DbType.String);
                _params.Add("@rfp_view_count", item.rfp_view_count, DbType.Int32);
                _params.Add("@rfp_change_user", user, DbType.String);
                _params.Add("@rfp_questionnaire", item.rfp_questionnaire, DbType.String);
                _params.Add("@rfp_evaluation_process", item.rfp_evaluation_process, DbType.String);
                _params.Add("@rfp_evaluation_criteria", item.rfp_evaluation_criteria, DbType.String);
                _params.Add("@rfp_terms_conditions", item.rfp_terms_conditions, DbType.String);
                _params.Add("@rfp_date_notes", item.rfp_date_notes, DbType.String);
                _params.Add("@rfp_published", item.rfp_published, DbType.Boolean);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_Request_Update", _params, null, null, CommandType.StoredProcedure);

                return new Response<RfpRequest>(item)
                {
                    Success = _params.Get<Int32>("success") == 1 ? true : false
                    //Message = _params.Get<String>("message")
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateData(DataObject item)
        {
            try
            {
                string sql = $"Update [rfp_request] SET {item.data_field_name}='{item.data_field_value}', rfp_change_user='{item.user_name}' WHERE {item.primary_key_name}='{item.primary_key_value}'";
                return _dBConnection.Execute(sql, null, null, null, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RfpOrgViewModel> SearchRfp(SearchFilter search)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Rfp_Request_Search", new
                {
                    Search = search.Text,
                    Types = string.IsNullOrWhiteSpace(search.Types) ? string.Empty : search.Types,
                    Sub_Types = string.IsNullOrWhiteSpace(search.Sub_Types) ? string.Empty : search.Sub_Types,
                    OrgKey = string.IsNullOrWhiteSpace(search.OrgKey) ? string.Empty : search.OrgKey,
                    OrgKey_search = string.IsNullOrWhiteSpace(search.OrgKey_search) ? string.Empty : search.OrgKey_search
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<RfpOrgViewModel>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RfpOrgViewModel> GetFavoriteRfp(string org_key)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Rfp_Request_Favorite", new
                {
                    OrgKey = string.IsNullOrWhiteSpace(org_key) ? string.Empty : org_key
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<RfpOrgViewModel>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RfpOrgViewModel> GetDashboardRfpForGovt(string org_key)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Rfp_Request_Get_Dashboard_Govt", new
                {
                    org_key = string.IsNullOrWhiteSpace(org_key) ? string.Empty : org_key
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<RfpOrgViewModel>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RfpReqOrgInvitationViewModel> GetGovtRFQs(string org_key)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Rfp_Request_Get_Govt_RFQ", new
                {
                    org_key = string.IsNullOrWhiteSpace(org_key) ? string.Empty : org_key
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<RfpReqOrgInvitationViewModel>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RfpReqResponse> GetRfpVendorResponse(string org_key)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Rfp_Vendor_Proposals", new
                {
                    OrgKey = string.IsNullOrWhiteSpace(org_key) ? string.Empty : org_key
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<RfpReqResponse>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<RfpRespondInvitation> SaveRfpRespondInvitation(RfpRespondInvitation item)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@inv_key", item.inv_key, DbType.Guid);
                _params.Add("@inv_rfp_key", item.inv_rfp_key, DbType.Guid);
                _params.Add("@inv_ct_key", item.inv_ct_key, DbType.Guid);
                _params.Add("@inv_first_name", item.inv_first_name, DbType.String);
                _params.Add("@inv_last_name", item.inv_last_name, DbType.String);
                _params.Add("@inv_full_name", item.inv_full_name, DbType.String);
                _params.Add("@inv_email", item.inv_email, DbType.String);
                _params.Add("@inv_status", item.inv_status, DbType.String);
                _params.Add("@inv_add_user", item.inv_add_user, DbType.String);
                _params.Add("@inv_change_user", item.inv_change_user, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_rfp_respond_invitation_Save", _params, null, null, CommandType.StoredProcedure);

                return new Response<RfpRespondInvitation>(item)
                {
                    Success = _params.Get<Int32>("success") == 1 ? true : false
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}