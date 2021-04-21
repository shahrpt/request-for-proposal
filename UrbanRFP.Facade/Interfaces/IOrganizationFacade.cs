using System.Collections.Generic;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Facade.Interfaces
{
    public interface IOrganizationFacade
    {
        Response<CoOrganization> Add(CoOrganization item, string user);

        Response<CoOrganization> Find(string id);

        List<CoOrganization> GetWhere(string query);

        List<CoOrganization> GetAll();
        
        bool Remove(string id, string user);

        Response<CoOrganization> Update(CoOrganization item, string user);
    }
}
