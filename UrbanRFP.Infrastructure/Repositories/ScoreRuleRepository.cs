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
    public class ScoreRuleRepository : IScoreRuleRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public ScoreRuleRepository(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }
        #endregion

        public Response<RfpScoreRule> Add(RfpScoreRule item, string user)
        {
            try
            {
                item.eva_key = Guid.NewGuid();
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@eva_key", item.eva_key, DbType.Guid);
                _params.Add("@eva_rfs_key", item.eva_rfs_key, DbType.Guid);
                _params.Add("@eva_rfp_key", item.eva_rfp_key, DbType.Guid);
                _params.Add("@eva_criteria_name", item.eva_criteria_name, DbType.String);
                _params.Add("@eva_criteria_description", item.eva_criteria_description, DbType.String);
                _params.Add("@eva_score_method", item.eva_score_method, DbType.String);
                _params.Add("@eva_point_max", item.eva_point_max, DbType.Int32);
                _params.Add("@eva_weight", item.eva_weight, DbType.Decimal);
                _params.Add("@eva_add_user", user, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_ScoreRules_Add", _params, null, null, CommandType.StoredProcedure);

                return new Response<RfpScoreRule>(item)
                {
                    Success = _params.Get<Int32>("success") == 1 ? true : false
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<RfpScoreRule> Find(string id)
        {
            throw new NotImplementedException();
        }

        public List<RfpScoreRule> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<RfpScoreRule> GetWhere(string search)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@Search", search, DbType.String);
               
                using (var result = _dBConnection.QueryMultiple("USP_Rfp_ScoreRules_GetWhere", _params, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<RfpScoreRule>().ToList();
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
                _params.Add("@eva_key", id, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_ScoreRules_Delete", _params, null, null, CommandType.StoredProcedure);

                return _params.Get<Int32>("success") == 1 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response<RfpScoreRule> Update(RfpScoreRule item, string user)
        {
            try
            {
                DynamicParameters _params = new DynamicParameters();
                _params.Add("@eva_key", item.eva_key, DbType.Guid);
                _params.Add("@eva_rfs_key", item.eva_rfs_key, DbType.Guid);
                _params.Add("@eva_rfp_key", item.eva_rfp_key, DbType.Guid);
                _params.Add("@eva_criteria_name", item.eva_criteria_name, DbType.String);
                _params.Add("@eva_criteria_description", item.eva_criteria_description, DbType.String);
                _params.Add("@eva_score_method", item.eva_score_method, DbType.String);
                _params.Add("@eva_point_max", item.eva_point_max, DbType.Int32);
                _params.Add("@eva_weight", item.eva_weight, DbType.Decimal);
                _params.Add("@eva_change_user", user, DbType.String);
                _params.Add("@success", DbType.Boolean, direction: ParameterDirection.Output);

                var result = _dBConnection.Execute("USP_Rfp_ScoreRules_Update", _params, null, null, CommandType.StoredProcedure);

                return new Response<RfpScoreRule>(item)
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
