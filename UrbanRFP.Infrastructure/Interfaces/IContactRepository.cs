using System;
using System.Collections.Generic;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Infrastructure.Interfaces
{
    public interface IContactRepository : IRepository<CoContact>
    {
        Response<UserProfile> GetByUsername(string userName);
        Response<UserProfile> GetByKey(string ct_key);
        Response<CoContact> Activate(string activation_code);
        Response<int> UpdateStatus(string ct_key, int status);
        Response<bool> UpdatePassword(string ct_key, string password);
        List<CoRole> GetRoles();
        List<RACI> GetRACI();
        void Attach_Contact_Request(string ct_key, string rfp_key, string role_id, string title, string user);
        void Add_RACI_Contact(string rxc_lk5_key, string rxc_ct_key, string rxc_rfp_key, string rxc_prd_key, string user);
        void Delete_RACI_Contact(string rxc_ct_key, string rxc_rfp_key, string rxc_prd_key);
        void Add_Contact_Org(string cxo_org_key, string cxo_ct_key, string user);
        List<ContactRACI> GetRACIContacts(RACIFilter filter);
        List<UserProfile> GetOrgContacts(Guid org_key);
    }
}
