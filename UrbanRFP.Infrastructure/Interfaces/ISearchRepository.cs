using System;
using System.Collections.Generic;
using System.Text;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Infrastructure.Interfaces
{
    public interface ISearchRepository //: IRepository<RfpProduct>
    {
        List<ProductOrganizationViewModel> FindRfpProducts(SearchFilter search);
        ProductOrganizationViewModel FindRfpProduct(string search);
    }
}
