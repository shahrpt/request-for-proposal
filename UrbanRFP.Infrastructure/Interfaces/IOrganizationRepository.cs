using System.Collections.Generic;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Infrastructure.Interfaces
{
    public interface IOrganizationRepository : IRepository<CoOrganization>
    {
        List<CoOrganization> GetWhere(string query);
    }
}
