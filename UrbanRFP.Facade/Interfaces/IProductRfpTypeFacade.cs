using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Facade.Interfaces
{
    public interface IProductRfpTypeFacade
    {
        Response<ProductRfpType> Add(ProductRfpType item, string user);
        Response<ProductRfpType> Find(string id);
        List<ProductRfpType> GetAll();
        List<ProductRfpType> GetWhere(string search);
        bool Remove(string id, string user);
        Response<ProductRfpType> Update(ProductRfpType item, string user);
    }
}
