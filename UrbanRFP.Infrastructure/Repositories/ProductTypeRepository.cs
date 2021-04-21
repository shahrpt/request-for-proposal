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
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public ProductTypeRepository(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public Response<ProductType> Add(ProductType item, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@id", DbType.Int32, direction: ParameterDirection.Output);
                _params.Add("@name", item.Name, DbType.String);

                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);
                //_params.Add("@message", DbType.String, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_ProductType_Add", _params, null, null, CommandType.StoredProcedure);
                item.Id = _params.Get<Int32>("id");

                return new Response<ProductType>(item)
                {
                    Success = _params.Get<Int32>("success") == 1? true : false
                    //Message = _params.Get<String>("message")
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<ProductType> Find(string id)
        {
            throw new NotImplementedException();
        }

        public List<ProductType> GetAll()
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_ProductType_GetWhere", new
                {
                    Search = ""
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<ProductType>().ToList();
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

                var result = _dBConnection.Execute("USP_ProductType_Delete", _params, null, null, CommandType.StoredProcedure);

                return _params.Get<Int32>("success") == 1 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<ProductType> Update(ProductType item, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@id", item.Id, DbType.Int32);
                _params.Add("@name", item.Name, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_ProductType_Update", _params, null, null, CommandType.StoredProcedure);
                
                return new Response<ProductType>(item)
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
