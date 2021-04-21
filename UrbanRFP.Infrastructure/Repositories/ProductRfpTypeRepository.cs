using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Infrastructure.Repositories
{
    public class ProductRfpTypeRepository : IProductRfpTypeRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public ProductRfpTypeRepository(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public Response<ProductRfpType> Add(ProductRfpType item, string user)
        {
            try
            {
                item.prc_key = Guid.NewGuid();
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@prc_key", item.prc_key.ToString(), DbType.Guid);
                _params.Add("@prc_rfp_key", item.prc_rfp_key.ToString(), DbType.Guid);
                _params.Add("@prc_prd_key", item.prc_prd_key, DbType.Guid);
                _params.Add("@prc_type_id", item.prc_type_id, DbType.Int32);
                _params.Add("@prc_subtype_id", item.prc_subtype_id, DbType.Int32);
                _params.Add("@prc_add_user", user, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_Product_Add", _params, null, null, CommandType.StoredProcedure);

                return new Response<ProductRfpType>(item)
                {
                    Success = _params.Get<Int32>("success") == 1 ? true : false
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<ProductRfpType> Find(string id)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@prc_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.QuerySingle<ProductRfpType>("USP_Product_Rfp_Type_Get", _params, null, null, CommandType.StoredProcedure);

                return new Response<ProductRfpType>(result)
                {
                    Success = result == null ? false : true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductRfpType> GetAll()
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_Product_Rfp_Type_GetWhere", new
                {
                    Search = string.Empty
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<ProductRfpType>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductRfpType> GetWhere(string search)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@Search", search, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                using (var result = _dBConnection.QueryMultiple("USP_Product_Rfp_Type_GetWhere", _params, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<ProductRfpType>().ToList();
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
                _params.Add("@prc_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Product_Rfp_Type_Delete", _params, null, null, CommandType.StoredProcedure);

                return _params.Get<Int32>("success") == 1 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<ProductRfpType> Update(ProductRfpType item, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@prc_key", item.prc_key.ToString(), DbType.Guid);
                _params.Add("@prc_rfp_key", item.prc_rfp_key.ToString(), DbType.Guid);
                _params.Add("@prc_prd_key", item.prc_prd_key, DbType.Guid);
                _params.Add("@prc_type_id", item.prc_type_id, DbType.Int32);
                _params.Add("@prc_subtype_id", item.prc_subtype_id, DbType.Int32);
                _params.Add("@prc_change_user", user, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Product_Rfp_Type_Update", _params, null, null, CommandType.StoredProcedure);

                return new Response<ProductRfpType>(item)
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
