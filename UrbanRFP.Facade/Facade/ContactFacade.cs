using System;
using System.Collections.Generic;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Facade.Facade
{
    public class ContactFacade : IContactFacade
    {
        private readonly IContactRepository _contactRepositoryDAC;

        #region CONSTRUCTORS
        public ContactFacade(IContactRepository contactRepositoryDAC)
        {
            _contactRepositoryDAC = contactRepositoryDAC ?? throw new ArgumentNullException(nameof(contactRepositoryDAC));
        }

        #endregion

        public Response<CoContact> Add(CoContact item, string user)
        {
            return _contactRepositoryDAC.Add(item, user);
        }

        public Response<CoContact> Find(string id)
        {
            return _contactRepositoryDAC.Find(id);
        }

        public Response<UserProfile> GetByKey(string ct_key)
        {
            var result = _contactRepositoryDAC.GetByKey(ct_key);

            if (result.Success)
            {
                result.Result.per_type = string.IsNullOrWhiteSpace(result.Result.per_type) ? "Public" : result.Result.per_type;
            }

            return result;
        }

        public List<CoContact> GetAll()
        {
            return _contactRepositoryDAC.GetAll();
        }

        public bool Remove(string id, string user)
        {
            return _contactRepositoryDAC.Remove(id, user);
        }

        public Response<CoContact> Update(CoContact item, string user)
        {
            return _contactRepositoryDAC.Update(item, user);
        }

        public Response<UserProfile> GetByUsername(string userName)
        {
            var result = _contactRepositoryDAC.GetByUsername(userName);

            if (result.Success)
            {
                result.Result.per_type = string.IsNullOrWhiteSpace(result.Result.per_type) ? "Public" : result.Result.per_type;
            }

            return result;
        }

        public Response<CoContact> Activate(string activation_code)
        {
            return _contactRepositoryDAC.Activate(activation_code);
        }

        public Response<int> UpdateStatus(string ct_key, int status)
        {
            return _contactRepositoryDAC.UpdateStatus(ct_key, status);
        }

        public Response<bool> UpdatePassword(string ct_key, string password)
        {
            return _contactRepositoryDAC.UpdatePassword(ct_key, password);
        }

        public List<CoRole> GetRoles()
        {
            return _contactRepositoryDAC.GetRoles();
        }

        public List<RACI> GetRACI()
        {
            return _contactRepositoryDAC.GetRACI();
        }

        public void Attach_Contact_Request(string ct_key, string rfp_key, string role_id, string title, string user)
        {
            _contactRepositoryDAC.Attach_Contact_Request(ct_key, rfp_key, role_id, title, user);
        }

        public void Add_RACI_Contact(string rxc_lk5_key, string rxc_ct_key, string rxc_rfp_key, string rxc_prd_key, string user)
        {
            _contactRepositoryDAC.Add_RACI_Contact(rxc_lk5_key, rxc_ct_key, rxc_rfp_key, rxc_prd_key, user);
        }

        public void Delete_RACI_Contact(string rxc_ct_key, string rxc_rfp_key, string rxc_prd_key)
        {
            _contactRepositoryDAC.Delete_RACI_Contact(rxc_ct_key, rxc_rfp_key, rxc_prd_key);
        }

        public void Add_Contact_Org(string cxo_org_key, string cxo_ct_key, string user)
        {
            _contactRepositoryDAC.Add_Contact_Org(cxo_org_key, cxo_ct_key, user);
        }

        public List<ContactRACI> GetRACIContacts(RACIFilter filter)
        {
            return _contactRepositoryDAC.GetRACIContacts(filter);
        }

        public List<UserProfile> GetOrgContacts(Guid org_key)
        {
            return _contactRepositoryDAC.GetOrgContacts(org_key);
        }
    }
}
