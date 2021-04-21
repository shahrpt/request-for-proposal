using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Infrastructure.Repositories
{
    public class RfpAttachmentRepository : IRfpAttachmentRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public RfpAttachmentRepository(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public Response<RfpAttachment> Add(RfpAttachment item, string user)
        {
            try
            {
                item.rfa_key = Guid.NewGuid();
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@rfa_key", item.rfa_key, DbType.Guid);
                _params.Add("@rfa_rfp_key", item.rfa_rfp_key, DbType.Guid);
                _params.Add("@rfa_rfr_key", item.rfa_rfr_key, DbType.Guid);
                _params.Add("@rfa_cxr_key", item.rfa_cxr_key, DbType.Guid);
                _params.Add("@rfa_prd_key", item.rfa_prd_key, DbType.Guid);
                _params.Add("@rfa_org_key", item.rfa_org_key, DbType.Guid);
                _params.Add("@rfa_file_name", item.rfa_file_name, DbType.String);
                _params.Add("@rfa_file_path", item.rfa_file_path, DbType.String);
                _params.Add("@rfa_type", item.rfa_type, DbType.String);
                _params.Add("@rfa_mime", item.rfa_mime, DbType.String);
                _params.Add("@rfa_attachment", item.rfa_attachment, DbType.Binary);
                _params.Add("@rfa_add_user", user, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_Attachment_Add", _params, null, null, CommandType.StoredProcedure);

                return new Response<RfpAttachment>(item)
                {
                    Success = _params.Get<Int32>("success") == 1 ? true : false
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<bool> UpdateField(string rfa_key, string fieldName, string fieldValue, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@rfa_key", rfa_key, DbType.String);
                _params.Add("@field_name", fieldName, DbType.String);
                _params.Add("@field_value", fieldValue, DbType.String);
                _params.Add("@rfa_change_user", user, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_Attachment_Update_Key_Value", _params, null, null, CommandType.StoredProcedure);

                return new Response<bool>(true)
                {
                    Success = _params.Get<Int32>("success") == 1 ? true : false
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<RfpAttachment> Find(string id)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@rfa_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.QuerySingle<RfpAttachment>("USP_Rfp_Attachment_Get", _params, null, null, CommandType.StoredProcedure);

                return new Response<RfpAttachment>(result)
                {
                    Success = result == null ? false : true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RfpAttachment> GetAll()
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Rfp_Attachment_GetWhere", new
                {
                    Search = ""
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<RfpAttachment>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RfpAttachment> GetWhere(string whereClause)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Rfp_Attachment_GetWhere", new
                {
                    Search = whereClause
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<RfpAttachment>().ToList();
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
                _params.Add("@rfa_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_Attachment_Delete", _params, null, null, CommandType.StoredProcedure);

                return _params.Get<Int32>("success") == 1 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<RfpAttachment> Update(RfpAttachment item, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@rfa_key", item.rfa_key, DbType.Guid);
                _params.Add("@rfa_rfp_key", item.rfa_rfp_key, DbType.Guid);
                _params.Add("@rfa_rfr_key", item.rfa_rfr_key, DbType.Guid);
                _params.Add("@rfa_cxr_key", item.rfa_cxr_key, DbType.Guid);
                _params.Add("@rfa_prd_key", item.rfa_prd_key, DbType.Guid);
                _params.Add("@rfa_org_key", item.rfa_org_key, DbType.Guid);
                _params.Add("@rfa_file_name", item.rfa_file_name, DbType.String);
                _params.Add("@rfa_file_path", item.rfa_file_path, DbType.String);
                _params.Add("@rfa_type", item.rfa_type, DbType.String);
                _params.Add("@rfa_mime", item.rfa_mime, DbType.String);
                _params.Add("@rfa_attachment", item.rfa_attachment, DbType.Binary);
                _params.Add("@rfa_change_user", user, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_Attachment_Update", _params, null, null, CommandType.StoredProcedure);

                return new Response<RfpAttachment>(item)
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

        public void Attach_RfpRequest_File(string rfa_key, string rfp_key)
        {
            try
            {
                _dBConnection.Execute($"INSERT INTO attachment_x_request(axr_key, axr_rfa_key, axr_rfp_key) VALUES('{Guid.NewGuid().ToString()}', '{rfa_key}', '{rfp_key}')", null, null, null, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}