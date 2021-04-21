using System.Collections.Generic;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Facade.Interfaces
{
    public interface IProductFacade
    {
        Response<RfpProduct> Add(RfpProduct item, string user);

        Response<RfpProduct> Find(string id);

        List<RfpProduct> GetAll();
        
        bool Remove(string id, string user);

        Response<RfpProduct> Update(RfpProduct item, string user);

        List<T> GetWhere<T>(string search);

        List<T> GetFavoriteProducts<T>(string org_key);
    }
}
