using System.Collections.Generic;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Facade.Interfaces
{
    public interface IProductTypeFacade
    {
        Response<ProductType> Add(ProductType item, string user);

        ProductType Find(string id);

        List<ProductType> GetAll();
        
        bool Remove(string id, string user);

        Response<ProductType> Update(ProductType item, string user);
    }
}
