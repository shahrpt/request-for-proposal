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
    public class ProductSubtypeRepository : IProductSubtypeRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public ProductSubtypeRepository(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public Response<ProductSubType> Add(ProductSubType item, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@id", DbType.Int32, direction: ParameterDirection.Output);
                _params.Add("@name", item.Name, DbType.String);
                _params.Add("@parentid", item.ParentID, DbType.Int32);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_ProductSubtype_Add", _params, null, null, CommandType.StoredProcedure);
                item.Id = _params.Get<Int32>("id");

                return new Response<ProductSubType>(item)
                {
                    Success = _params.Get<Int32>("success") == 1 ? true : false
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<ProductSubType> Find(string id)
        {
            throw new NotImplementedException();
        }

        public List<ProductSubType> GetAll()
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_ProductSubtype_GetWhere", new
                {
                    Search = ""
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<ProductSubType>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductSubType> GetByParentId(int parentId)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_ProductSubtype_GetWhere", new
                {
                    Search = "parentid=" + parentId
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<ProductSubType>().ToList();
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
                _params.Add("@id", id, DbType.Int32);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_ProductSubtype_Delete", _params, null, null, CommandType.StoredProcedure);

                return _params.Get<Int32>("success") == 1 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<ProductSubType> Update(ProductSubType item, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@id", item.Id, DbType.Int32);
                _params.Add("@parentid", item.ParentID, DbType.Int32);
                _params.Add("@name", item.Name, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_ProductSubtype_Update", _params, null, null, CommandType.StoredProcedure);

                return new Response<ProductSubType>(item)
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
