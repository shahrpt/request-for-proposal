using System.Collections.Generic;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Facade.Interfaces
{
    public interface IProductSubtypeFacade
    {
        Response<ProductSubType> Add(ProductSubType item, string user);

        ProductSubType Find(string id);

        List<ProductSubType> GetAll();
        
        bool Remove(string id, string user);

        Response<ProductSubType> Update(ProductSubType item, string user);

        List<ProductSubType> GetByParentId(int parentId);
    }
}
