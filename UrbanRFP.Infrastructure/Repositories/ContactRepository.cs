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
    public class ContactRepository : IContactRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public ContactRepository(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        #endregion

        public Response<CoContact> Add(CoContact item, string user)
        {
            try
            {
                item.ct_key = Guid.NewGuid();
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@ct_key", item.ct_key.ToString(), DbType.String);
                _params.Add("@ct_first_name", item.ct_first_name.ToString(), DbType.String);
                _params.Add("@ct_last_name", item.ct_last_name, DbType.String);
                _params.Add("@ct_email", item.ct_email, DbType.String);
                _params.Add("@ct_phone", item.ct_phone, DbType.String);
                _params.Add("@ct_fax", item.ct_fax, DbType.String);
                _params.Add("@ct_password", item.ct_password, DbType.String);
                _params.Add("@ct_add_user", item.ct_add_user, DbType.String);
                _params.Add("@ct_activation_code", item.ct_activation_code.ToString(), DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Co_Contact_Add", _params, null, null, CommandType.StoredProcedure);

                return new Response<CoContact>(item)
                {
                    Success = _params.Get<Int32>("success") == 1 ? true : false
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<CoContact> Find(string id)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@ct_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.QuerySingle<CoContact>("USP_Co_Contact_Get", _params, null, null, CommandType.StoredProcedure);

                return new Response<CoContact>(result)
                {
                    Success = result == null ? false : true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<UserProfile> GetByKey(string ct_key)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@ct_key", ct_key, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.QuerySingle<UserProfile>("USP_Co_Contact_Get", _params, null, null, CommandType.StoredProcedure);

                return new Response<UserProfile>(result)
                {
                    Success = result == null ? false : true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CoContact> GetAll()
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Co_Contact_GetWhere", new
                {
                    Search = ""
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<CoContact>().ToList();
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
                _params.Add("@ct_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Co_Contact_Delete", _params, null, null, CommandType.StoredProcedure);

                return _params.Get<Int32>("success") == 1 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<CoContact> Update(CoContact item, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@ct_key", item.ct_key.ToString(), DbType.String);
                _params.Add("@ct_first_name", item.ct_first_name.ToString(), DbType.String);
                _params.Add("@ct_last_name", item.ct_last_name, DbType.String);
                _params.Add("@ct_email", item.ct_email, DbType.String);
                _params.Add("@ct_phone", item.ct_phone, DbType.String);
                _params.Add("@ct_fax", item.ct_fax, DbType.String);
                _params.Add("@ct_password", item.ct_password, DbType.String);
                _params.Add("@org_change_user", user, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Co_Contact_Update", _params, null, null, CommandType.StoredProcedure);

                return new Response<CoContact>(item)
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

        public Response<UserProfile> GetByUsername(string userName)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Co_Contact_GetWhere", new
                {
                    Search = $"ct_email='{userName}'"
                }, commandType: CommandType.StoredProcedure))
                {
                    var list  = result.Read<UserProfile>().ToList();

                    if (list != null && list.Count > 0)
                    {
                        return new Response<UserProfile>(list[0]) { Success = true };
                    }
                    else
                    {
                        return new Response<UserProfile>(null) { Success = false };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<CoContact> Activate(string activation_code)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Co_Contact_GetWhere", new
                {
                    Search = $"ct_activation_code='{activation_code}'"
                }, commandType: CommandType.StoredProcedure))
                {
                    var list = result.Read<CoContact>().ToList();

                    if (list != null && list.Count > 0)
                    {
                        var contact = list[0];

                        var status_resp = UpdateStatus(contact.ct_key.ToString(), 1);

                        if (status_resp.Success)
                            return new Response<CoContact>(contact) { Success = true };
                        else
                            return new Response<CoContact>(null) { Success = false };
                    }
                    else
                    {
                        return new Response<CoContact>(null) { Success = false };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<int> UpdateStatus(string ct_key, int status)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@ct_key", ct_key, DbType.String);
                _params.Add("@status", status, DbType.Int16);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Co_Contact_Change_Status", _params, null, null, CommandType.StoredProcedure);

                return new Response<int>(status)
                {
                    Success = _params.Get<Int32>("success") == 1 ? true : false
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<bool> UpdatePassword(string ct_key, string password)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@ct_key", ct_key, DbType.String);
                _params.Add("@ct_password", password, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Co_Contact_Change_Password", _params, null, null, CommandType.StoredProcedure);

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

        public List<CoRole> GetRoles()
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("SELECT * FROM lkup_cxr_rlt_code", null, commandType: CommandType.Text))
                {
                    return result.Read<CoRole>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RACI> GetRACI()
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("SELECT * FROM lkup_RACI", null, commandType: CommandType.Text))
                {
                    return result.Read<RACI>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ContactRACI> GetRACIContacts(RACIFilter filter)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Co_Contact_GetRACI", filter, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<ContactRACI>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserProfile> GetOrgContacts(Guid org_key)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Co_Contact_Get_Org", new { Org_key = org_key }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<UserProfile>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Attach_Contact_Request(string ct_key, string rfp_key, string role_id, string title, string user)
        {
            try
            {
                _dBConnection.Execute($"INSERT INTO contact_x_request(cxp_key, cxp_ct_key, cxp_rfp_key, cxp_rlt_code, cxp_title, cxp_add_date, cxp_add_user)" +
                                    $" VALUES('{Guid.NewGuid().ToString()}', '{ct_key}', '{rfp_key}', '{role_id}', '{title}', GetDate(), '{user}')", null, null, null, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Add_RACI_Contact(string rxc_lk5_key, string rxc_ct_key, string rxc_rfp_key, string rxc_prd_key, string user)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(rxc_rfp_key))
                {
                    _dBConnection.Execute($"INSERT INTO contact_x_RACI(rxc_key, rxc_lk5_key, rxc_ct_key, rxc_rfp_key, rxc_add_date, rxc_add_user)" +
                                    $" VALUES('{Guid.NewGuid().ToString()}', '{rxc_lk5_key}', '{rxc_ct_key}', '{rxc_rfp_key}', GetDate(), '{user}')", null, null, null, CommandType.Text);
                }
                else if (!string.IsNullOrWhiteSpace(rxc_prd_key))
                {
                    _dBConnection.Execute($"INSERT INTO contact_x_RACI(rxc_key, rxc_lk5_key, rxc_ct_key, rxc_prd_key, rxc_add_date, rxc_add_user)" +
                                    $" VALUES('{Guid.NewGuid().ToString()}', '{rxc_lk5_key}', '{rxc_ct_key}', '{rxc_prd_key}', GetDate(), '{user}')", null, null, null, CommandType.Text);
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete_RACI_Contact(string rxc_ct_key, string rxc_rfp_key, string rxc_prd_key)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(rxc_rfp_key))
                {
                    _dBConnection.Execute($"UPDATE contact_x_RACI SET rxc_delete_flag=1 WHERE rxc_rfp_key='{rxc_rfp_key}' AND rxc_ct_key='{rxc_ct_key}'", null, null, null, CommandType.Text);
                }
                else if (!string.IsNullOrWhiteSpace(rxc_prd_key)) {
                    _dBConnection.Execute($"UPDATE contact_x_RACI SET rxc_delete_flag=1 WHERE rxc_prd_key='{rxc_prd_key}' AND rxc_ct_key='{rxc_ct_key}'", null, null, null, CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Add_Contact_Org(string cxo_org_key, string cxo_ct_key, string user)
        {
            try
            {
                _dBConnection.Execute($"INSERT INTO contact_x_organization(cxo_key, cxo_org_key, cxo_ct_key, cxo_add_date, cxo_add_user)" +
                                    $" VALUES('{Guid.NewGuid().ToString()}', '{cxo_org_key}', '{cxo_ct_key}', GetDate(), '{user}')", null, null, null, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}