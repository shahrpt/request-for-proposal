using System;
using System.Collections.Generic;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Facade.Facade
{
    public class ProductRfpTypeFacade : IProductRfpTypeFacade
    {
        private readonly IProductRfpTypeRepository _productRfpTypeRepositoryDAC;

        #region CONSTRUCTORS
        public ProductRfpTypeFacade(IProductRfpTypeRepository productRfpTypeRepositoryDAC)
        {
            _productRfpTypeRepositoryDAC = productRfpTypeRepositoryDAC ?? throw new ArgumentNullException(nameof(productRfpTypeRepositoryDAC));
        }

        #endregion

        public Response<ProductRfpType> Add(ProductRfpType item, string user)
        {
            return _productRfpTypeRepositoryDAC.Add(item, user);
        }

        public Response<ProductRfpType> Find(string id)
        {
            return _productRfpTypeRepositoryDAC.Find(id);
        }

        public List<ProductRfpType> GetAll()
        {
            return _productRfpTypeRepositoryDAC.GetAll();
        }

        public List<ProductRfpType> GetWhere(string search)
        {
            return _productRfpTypeRepositoryDAC.GetWhere(search);
        }

        public bool Remove(string id, string user)
        {
            return _productRfpTypeRepositoryDAC.Remove(id, user);
        }

        public Response<ProductRfpType> Update(ProductRfpType item, string user)
        {
            return _productRfpTypeRepositoryDAC.Update(item, user);
        }
    }
}
