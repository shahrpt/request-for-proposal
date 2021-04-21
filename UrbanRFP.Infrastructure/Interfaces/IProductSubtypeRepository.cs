using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Infrastructure.Interfaces
{
    public interface IProductSubtypeRepository : IRepository<ProductSubType>
    {
        List<ProductSubType> GetByParentId(int parentId);
    }
}
