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
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public OrganizationRepository(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public Response<CoOrganization> Add(CoOrganization item, string user)
        {
            try
            {
                item.org_key = Guid.NewGuid();
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@org_key", item.org_key.ToString(), DbType.String);
                _params.Add("@org_legal_name", item.org_legal_name.ToString(), DbType.String);
                _params.Add("@org_description", item.org_description, DbType.String);
                _params.Add("@org_specialities", item.org_specialities, DbType.String);
                _params.Add("@org_vendor_flag", item.org_vendor_flag, DbType.Byte);
                _params.Add("@org_date_created", item.org_date_created, DbType.DateTime);
                _params.Add("@org_timezone", item.org_timezone, DbType.String);
                _params.Add("@org_entity_type", item.org_entity_type, DbType.String);
                _params.Add("@org_special_type_flag", item.org_special_type_flag, DbType.Byte);
                _params.Add("@org_qualified_LG_flag", item.org_qualified_LG_flag, DbType.Byte);
                _params.Add("@org_federal_tax_id", item.org_federal_tax_id, DbType.String);
                _params.Add("@org_website", item.org_website, DbType.String);
                _params.Add("@org_ann_revenue", item.org_ann_revenue, DbType.Double);
                _params.Add("@org_vendor_number_of_employees", item.org_vendor_number_of_employees, DbType.Int32);
                _params.Add("@org_LG_population_range", item.org_LG_population_range, DbType.Int32);
                _params.Add("@org_add_user", item.org_add_user, DbType.String);
                _params.Add("@org_domain", item.org_domain, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Co_Organization_Add", _params, null, null, CommandType.StoredProcedure);

                return new Response<CoOrganization>(item)
                {
                    Success = _params.Get<Int32>("success") == 1 ? true : false
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<CoOrganization> Find(string id)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@org_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.QuerySingle<CoOrganization>("USP_Co_Organization_Get", _params, null, null, CommandType.StoredProcedure);

                return new Response<CoOrganization>(result)
                {
                    Success = result == null ? false : true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CoOrganization> GetAll()
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Co_Organization_GetWhere", new
                {
                    Search = ""
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<CoOrganization>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CoOrganization> GetWhere(string query)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Co_Organization_GetWhere", new
                {
                    Search = query
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<CoOrganization>().ToList();
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
                _params.Add("@org_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Co_Organization_Delete", _params, null, null, CommandType.StoredProcedure);

                return _params.Get<Int32>("success") == 1 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<CoOrganization> Update(CoOrganization item, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@org_key", item.org_key.ToString(), DbType.String);
                _params.Add("@org_legal_name", item.@org_legal_name.ToString(), DbType.String);
                _params.Add("@org_description", item.org_description, DbType.String);
                _params.Add("@org_specialities", item.org_specialities, DbType.String);
                _params.Add("@org_vendor_flag", item.org_vendor_flag, DbType.Byte);
                _params.Add("@org_date_created", item.org_date_created, DbType.DateTime);
                _params.Add("@org_timezone", item.org_timezone, DbType.String);
                _params.Add("@org_entity_type", item.org_entity_type, DbType.String);
                _params.Add("@org_special_type_flag", item.org_special_type_flag, DbType.Byte);
                _params.Add("@org_qualified_LG_flag", item.org_qualified_LG_flag, DbType.Byte);
                _params.Add("@org_federal_tax_id", item.org_federal_tax_id, DbType.String);
                _params.Add("@org_website", item.org_website, DbType.String);
                _params.Add("@org_ann_revenue", item.org_ann_revenue, DbType.Double);
                _params.Add("@org_vendor_number_of_employees", item.org_vendor_number_of_employees, DbType.Int32);
                _params.Add("@org_LG_population_range", item.org_LG_population_range, DbType.Int32);
                _params.Add("@org_change_user", user, DbType.String);
                _params.Add("@org_domain", item.org_domain, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Co_Organization_Update", _params, null, null, CommandType.StoredProcedure);

                return new Response<CoOrganization>(item)
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
        #endregion
    }
}
