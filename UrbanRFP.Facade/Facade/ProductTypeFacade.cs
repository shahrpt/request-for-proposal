using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Facade.Facade
{
    public class ProductTypeFacade : IProductTypeFacade
    {
        private readonly IProductTypeRepository _productTypeRepositoryDAC;

        #region CONSTRUCTORS
        public ProductTypeFacade(IProductTypeRepository productTypeRepositoryDAC)
        {
            _productTypeRepositoryDAC = productTypeRepositoryDAC ?? throw new ArgumentNullException(nameof(productTypeRepositoryDAC));
        }

        #endregion

        public Response<ProductType> Add(ProductType item, string user)
        {
            return _productTypeRepositoryDAC.Add(item, user);
        }

        public ProductType Find(string id)
        {
            throw new NotImplementedException();
        }

        public List<ProductType> GetAll()
        {
            return _productTypeRepositoryDAC.GetAll();
        }

        public bool Remove(string id, string user)
        {
            return _productTypeRepositoryDAC.Remove(id, user);
        }

        public Response<ProductType> Update(ProductType item, string user)
        {
            return _productTypeRepositoryDAC.Update(item, user);
        }
    }
}