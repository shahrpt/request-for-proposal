using System;
using System.Collections.Generic;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Facade.Facade
{
    public class OrganizationFacade : IOrganizationFacade
    {
        private readonly IOrganizationRepository _organizationRepositoryDAC;

        #region CONSTRUCTORS
        public OrganizationFacade(IOrganizationRepository organizationRepositoryDAC)
        {
            _organizationRepositoryDAC = organizationRepositoryDAC ?? throw new ArgumentNullException(nameof(organizationRepositoryDAC));
        }

        #endregion

        public Response<CoOrganization> Add(CoOrganization item, string user)
        {
            return _organizationRepositoryDAC.Add(item, user);
        }

        public Response<CoOrganization> Find(string id)
        {
            return _organizationRepositoryDAC.Find(id);
        }

        public List<CoOrganization> GetAll()
        {
            return _organizationRepositoryDAC.GetAll();
        }

        public List<CoOrganization> GetWhere(string query)
        {
            return _organizationRepositoryDAC.GetWhere(query);
        }

        public bool Remove(string id, string user)
        {
            return _organizationRepositoryDAC.Remove(id, user);
        }

        public Response<CoOrganization> Update(CoOrganization item, string user)
        {
            return _organizationRepositoryDAC.Update(item, user);
        }
    }
}