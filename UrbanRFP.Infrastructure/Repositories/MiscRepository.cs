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
    public class MiscRepository : IMiscRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public MiscRepository(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public Response<int> ViewPage(CoPageView item)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@pv_key", item.pv_key, DbType.Guid);
                _params.Add("@pv_page_id", item.pv_page_id, DbType.Int32);
                _params.Add("@pv_prd_key", item.pv_prd_key, DbType.Guid);
                _params.Add("@pv_rfp_key", item.pv_rfp_key, DbType.Guid);
                _params.Add("@pv_source_ct_key", item.pv_source_ct_key, DbType.Guid);
                _params.Add("@pv_add_user", item.pv_add_user, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Co_Page_View_Add", _params, null, null, CommandType.StoredProcedure);

                return new Response<int>(result)
                {
                    Success = _params.Get<Int32>("success") == 1 ? true : false
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<int> MarkFavorite(CoFavorite item)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@fav_key", item.fav_key, DbType.Guid);
                _params.Add("@fav_org_key", item.fav_org_key, DbType.Guid);
                _params.Add("@fav_prd_key", item.fav_prd_key, DbType.Guid);
                _params.Add("@fav_rfp_key", item.fav_rfp_key, DbType.Guid);
                _params.Add("@fav_add_user", item.fav_add_user, DbType.String);
                _params.Add("@is_favorite", item.is_favorite ? 1 : 0, DbType.Int16);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Co_Favorite_Set", _params, null, null, CommandType.StoredProcedure);

                return new Response<int>(result)
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
