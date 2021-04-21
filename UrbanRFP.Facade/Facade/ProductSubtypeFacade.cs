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
    public class ProductSubtypeFacade : IProductSubtypeFacade
    {
        private readonly IProductSubtypeRepository _productSubtypeRepositoryDAC;

        #region CONSTRUCTORS
        public ProductSubtypeFacade(IProductSubtypeRepository productSubtypeRepositoryDAC)
        {
            _productSubtypeRepositoryDAC = productSubtypeRepositoryDAC ?? throw new ArgumentNullException(nameof(productSubtypeRepositoryDAC));
        }

        #endregion

        public Response<ProductSubType> Add(ProductSubType item, string user)
        {
            return _productSubtypeRepositoryDAC.Add(item, user);
        }

        public ProductSubType Find(string id)
        {
            throw new NotImplementedException();
        }

        public List<ProductSubType> GetAll()
        {
            return _productSubtypeRepositoryDAC.GetAll();
        }

        public List<ProductSubType> GetByParentId(int parentId)
        {
            return _productSubtypeRepositoryDAC.GetByParentId(parentId);
        }

        public bool Remove(string id, string user)
        {
            return _productSubtypeRepositoryDAC.Remove(id, user);
        }

        public Response<ProductSubType> Update(ProductSubType item, string user)
        {
            return _productSubtypeRepositoryDAC.Update(item, user);
        }
    }
}