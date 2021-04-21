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
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public ProductRepository(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public Response<RfpProduct> Add(RfpProduct item, string user)
        {
            try
            {
                item.prd_key = Guid.NewGuid();
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@prd_key", item.prd_key.ToString(),DbType.String);
                _params.Add("@prd_org_key", item.prd_org_key.ToString(), DbType.String);
                _params.Add("@prd_name", item.prd_name, DbType.String);
                _params.Add("@prd_type", item.prd_type, DbType.Int32);
                _params.Add("@prd_subtype", item.prd_subtype, DbType.Int32);
                _params.Add("@prd_description", item.prd_description, DbType.String);
                _params.Add("@prd_benefits", item.prd_benefits, DbType.String);
                _params.Add("@prd_use_cases", item.prd_use_cases, DbType.String);
                _params.Add("@prd_pricing", item.prd_pricing, DbType.String);
                _params.Add("@prd_features", item.prd_features, DbType.String);
                _params.Add("@prd_website", item.prd_website, DbType.String);
                _params.Add("@prd_target_users", item.prd_target_users, DbType.String);
                _params.Add("@prd_add_date", item.prd_add_date, DbType.DateTime);
                _params.Add("@prd_add_user", item.prd_add_user, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_Product_Add", _params, null, null, CommandType.StoredProcedure);

                return new Response<RfpProduct>(item)
                {
                    Success = _params.Get<Int32>("success") == 1? true : false
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<RfpProduct> Find(string id)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@prd_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.QuerySingle< RfpProduct>("USP_Rfp_Product_Get", _params, null, null, CommandType.StoredProcedure);

                return new Response<RfpProduct>(result)
                {
                    Success = result == null ? false : true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RfpProduct> GetAll()
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Rfp_Product_GetWhere", new
                {
                    Search = string.Empty
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<RfpProduct>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<T> GetWhere<T>(string search)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@Search", search, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                using (var result = _dBConnection.QueryMultiple("USP_Rfp_Product_GetWhere", _params, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<T>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> GetFavoriteProducts<T>(string org_key)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@OrgKey", org_key, DbType.String);

                using (var result = _dBConnection.QueryMultiple("USP_Rfp_Product_Favorite", _params, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<T>().ToList();
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
                _params.Add("@prd_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_Product_Delete", _params, null, null, CommandType.StoredProcedure);

                return _params.Get<Int32>("success") == 1 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<RfpProduct> Update(RfpProduct item, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@prd_key", item.prd_key.ToString(), DbType.String);
                _params.Add("@prd_org_key", item.prd_org_key.ToString(), DbType.String);
                _params.Add("@prd_name", item.prd_name, DbType.String);
                _params.Add("@prd_type", item.prd_type, DbType.Int32);
                _params.Add("@prd_subtype", item.prd_subtype, DbType.Int32);
                _params.Add("@prd_description", item.prd_description, DbType.String);
                _params.Add("@prd_benefits", item.prd_benefits, DbType.String);
                _params.Add("@prd_use_cases", item.prd_use_cases, DbType.String);
                _params.Add("@prd_pricing", item.prd_pricing, DbType.String);
                _params.Add("@prd_features", item.prd_features, DbType.String);
                _params.Add("@prd_website", item.prd_website, DbType.String);
                _params.Add("@prd_target_users", item.prd_target_users, DbType.String);
                _params.Add("@prd_change_date", DateTime.Now, DbType.DateTime);
                _params.Add("@prd_change_user", user, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_Product_Update", _params, null, null, CommandType.StoredProcedure);
                
                return new Response<RfpProduct>(item)
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
    }
}
