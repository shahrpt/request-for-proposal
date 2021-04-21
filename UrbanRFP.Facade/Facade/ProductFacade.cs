using System;
using System.Collections.Generic;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Facade.Facade
{
    public class ProductFacade : IProductFacade
    {
        private readonly IProductRepository _productRepositoryDAC;

        #region CONSTRUCTORS
        public ProductFacade(IProductRepository productRepositoryDAC)
        {
            _productRepositoryDAC = productRepositoryDAC ?? throw new ArgumentNullException(nameof(productRepositoryDAC));
        }

        #endregion

        public Response<RfpProduct> Add(RfpProduct item, string user)
        {
            return _productRepositoryDAC.Add(item, user);
        }

        public Response<RfpProduct> Find(string id)
        {
            return _productRepositoryDAC.Find(id);
        }

        public List<RfpProduct> GetAll()
        {
            return _productRepositoryDAC.GetAll();
        }

        public bool Remove(string id, string user)
        {
            return _productRepositoryDAC.Remove(id, user);
        }

        public Response<RfpProduct> Update(RfpProduct item, string user)
        {
            return _productRepositoryDAC.Update(item, user);
        }

        public List<T> GetWhere<T>(string search)
        {
            return _productRepositoryDAC.GetWhere<T>(search);
        }

        public List<T> GetFavoriteProducts<T>(string org_key)
        {
            return _productRepositoryDAC.GetFavoriteProducts<T>(org_key);
        }
    }
}