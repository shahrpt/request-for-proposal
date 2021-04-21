using System;
using System.Collections.Generic;
using System.Text;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Facade.Interfaces
{
    public interface ISearchFacade
    {
        List<ProductOrganizationViewModel> FindRfpProducts(SearchFilter search);
        ProductOrganizationViewModel FindRfpProduct(string search);
    }
}
