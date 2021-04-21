using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Infrastructure.Repositories
{
    public class GeneralRepository : IGeneralRepository
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public GeneralRepository(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        #endregion

        public List<SystemMenu> GetMenuItems()
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_GetMenuItems", new
                {
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<SystemMenu>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
