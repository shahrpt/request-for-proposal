using System;
using System.Collections.Generic;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Facade.Facade
{
    public class RfpRequestFacade : IRfpRequestFacade
    {
        private readonly IRfpRequestRepository _rfpRequestRepositoryDAC;

        #region CONSTRUCTORS
        public RfpRequestFacade(IRfpRequestRepository rfpRequestRepositoryDAC)
        {
            _rfpRequestRepositoryDAC = rfpRequestRepositoryDAC ?? throw new ArgumentNullException(nameof(rfpRequestRepositoryDAC));
        }

        #endregion

        public Response<RfpRequest> Add(RfpRequest item, string user)
        {
            return _rfpRequestRepositoryDAC.Add(item, user);
        }

        public Response<RfpRequest> Find(string id)
        {
            return _rfpRequestRepositoryDAC.Find(id);
        }

        public List<RfpRequest> GetAll()
        {
            return _rfpRequestRepositoryDAC.GetAll();
        }

        public bool Remove(string id, string user)
        {
            return _rfpRequestRepositoryDAC.Remove(id, user);
        }

        public Response<RfpRequest> Update(RfpRequest item, string user)
        {
            return _rfpRequestRepositoryDAC.Update(item, user);
        }

        public int UpdateData(DataObject item)
        {
            return _rfpRequestRepositoryDAC.UpdateData(item);
        }

        public List<RfpOrgViewModel> SearchRfp(SearchFilter search)
        {
            return _rfpRequestRepositoryDAC.SearchRfp(search);
        }

        public List<RfpOrgViewModel> GetFavoriteRfp(string org_key)
        {
            return _rfpRequestRepositoryDAC.GetFavoriteRfp(org_key);
        }

        public List<RfpReqResponse> GetRfpVendorResponse(string org_key)
        {
            return _rfpRequestRepositoryDAC.GetRfpVendorResponse(org_key);
        }

        public List<RfpOrgViewModel> GetDashboardRfpForGovt(string org_key)
        {
            return _rfpRequestRepositoryDAC.GetDashboardRfpForGovt(org_key);
        }

        public List<RfpReqOrgInvitationViewModel> GetGovtRFQs(string org_key)
        {
            return _rfpRequestRepositoryDAC.GetGovtRFQs(org_key);
        }

        public List<RfpRequest> GetWhere(string search)
        {
            return _rfpRequestRepositoryDAC.GetWhere(search);
        }

        public RfpRequest GetRFQDetails(string org_key,  string prd_key)
        {
            var result = _rfpRequestRepositoryDAC.GetWhere($"(rfp_request.rfp_prd_key='{prd_key}' AND rfp_request.rfp_org_key='{org_key}')");
            return (result != null && result.Count > 0 && !string.IsNullOrWhiteSpace(result[0].rfp_name)) ? result[0] : null;
        }

        public Response<RfpRespondInvitation> SaveRfpRespondInvitation(RfpRespondInvitation item)
        {
            return _rfpRequestRepositoryDAC.SaveRfpRespondInvitation(item);
        }
    }
}