using System.Collections.Generic;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Facade.Interfaces
{
    public interface IRfpRequestFacade
    {
        Response<RfpRequest> Add(RfpRequest item, string user);
        Response<RfpRequest> Find(string id);
        List<RfpRequest> GetAll();
        bool Remove(string id, string user);
        Response<RfpRequest> Update(RfpRequest item, string user);
        int UpdateData(DataObject item);
        List<RfpOrgViewModel> SearchRfp(SearchFilter search);
        List<RfpOrgViewModel> GetFavoriteRfp(string org_key);
        List<RfpReqResponse> GetRfpVendorResponse(string org_key);
        List<RfpOrgViewModel> GetDashboardRfpForGovt(string org_key);
        List<RfpReqOrgInvitationViewModel> GetGovtRFQs(string org_key);
        List<RfpRequest> GetWhere(string search);
        RfpRequest GetRFQDetails(string org_key, string prd_key);
        Response<RfpRespondInvitation> SaveRfpRespondInvitation(RfpRespondInvitation item);
    }
}
