using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;
using UrbanRFP.Domain.ViewModel;

namespace UrbanRFP.Infrastructure.Repositories
{
    public class SearchRepository : ISearchRepository // ,RepositoryBase<RfpProduct>,
    {
        private readonly IDbConnection _dBConnection;

        #region CONSTRUCTORS
        public SearchRepository(IDbConnection dbConnection)
        {
            _dBConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        #endregion

        public List<ProductOrganizationViewModel> FindRfpProducts(SearchFilter search)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_GetSearchRfpProducts", new
                {
                    Search = search.Text,
                    Types = string.IsNullOrWhiteSpace(search.Types) ? string.Empty : search.Types,
                    Sub_Types = string.IsNullOrWhiteSpace(search.Sub_Types) ? string.Empty : search.Sub_Types,
                    OrgKey = string.IsNullOrWhiteSpace(search.OrgKey) ? string.Empty : search.OrgKey,
                    OrgKey_search = string.IsNullOrWhiteSpace(search.OrgKey_search) ? string.Empty : search.OrgKey_search
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<ProductOrganizationViewModel>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ProductOrganizationViewModel FindRfpProduct(string search)
        {
            try
            {
                using (var result = _dBConnection.QueryMultiple("USP_GetSearchRfpOrganization", new
                {
                    Search = search
                }, commandType: CommandType.StoredProcedure))
                {
                    return result.Read<ProductOrganizationViewModel>().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
