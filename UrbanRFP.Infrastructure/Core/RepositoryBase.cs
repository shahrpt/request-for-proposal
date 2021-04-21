using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanRFP.Infrastructure.Core
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        public abstract Response<T> Add(T item, string user);
        public abstract Response<T> Find(string id);
        public abstract List<T> GetAll();
        public abstract bool Remove(string id, string user);
        public abstract Response<T> Update(T item, string user);
    }
}
