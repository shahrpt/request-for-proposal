using System.Collections.Generic;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Infrastructure.Interfaces
{
    public interface IRfpRequestRepository : IRepository<RfpRequest>
    {
        int UpdateData(DataObject item);
        List<RfpOrgViewModel> SearchRfp(SearchFilter search);
        List<RfpOrgViewModel> GetFavoriteRfp(string org_key);
        List<RfpReqResponse> GetRfpVendorResponse(string org_key);
        List<RfpOrgViewModel> GetDashboardRfpForGovt(string org_key);
        List<RfpReqOrgInvitationViewModel> GetGovtRFQs(string org_key);
        List<RfpRequest> GetWhere(string search);
        Response<RfpRespondInvitation> SaveRfpRespondInvitation(RfpRespondInvitation item);
    }
}
