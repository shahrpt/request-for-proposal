using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanRFP.Infrastructure.Core
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        Response<T> Find(string id);
        Response<T> Add(T item, string user);
        bool Remove(string id, string user);
        Response<T> Update(T item, string user);
    }
}
