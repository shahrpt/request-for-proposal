using System.Collections.Generic;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Infrastructure.Interfaces
{
    public interface IProductRepository : IRepository<RfpProduct>
    {
        List<T> GetWhere<T>(string search);
        List<T> GetFavoriteProducts<T>(string org_key);
    }
}
