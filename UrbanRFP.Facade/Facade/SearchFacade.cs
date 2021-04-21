using System;
using System.Collections.Generic;
using System.Text;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;
using UrbanRPF.Mapping;

namespace UrbanRFP.Facade.Facade
{
    public class SearchFacade : ISearchFacade
    {
        private readonly ISearchRepository _searchRepositoryDAC;

        #region CONSTRUCTORS
        public SearchFacade(ISearchRepository searchRepositoryDAC)
        {
            _searchRepositoryDAC = searchRepositoryDAC ?? throw new ArgumentNullException(nameof(searchRepositoryDAC));
        }

        public ProductOrganizationViewModel FindRfpProduct(string search)
        {
            return _searchRepositoryDAC.FindRfpProduct(search);
        }

        public List<ProductOrganizationViewModel> FindRfpProducts(SearchFilter search)
        {
            return _searchRepositoryDAC.FindRfpProducts(search);
        }
        //public List<ProductOrganizationViewModel> FindRfpProducts(string search) => AutoMapperHelper<ProductOrganizationViewModel, RfpProduct>.MapList(_userRepositoryDAC.FindRfpProducts(search));

        #endregion
    }
}
