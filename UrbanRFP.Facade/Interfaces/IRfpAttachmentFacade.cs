using System.Collections.Generic;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Facade.Interfaces
{
    public interface IRfpAttachmentFacade
    {
        Response<RfpAttachment> Add(RfpAttachment item, string user);

        Response<RfpAttachment> Find(string id);

        List<RfpAttachment> GetAll();

        List<RfpAttachment> GetWhere(string whereClause);

        bool Remove(string id, string user);

        Response<RfpAttachment> Update(RfpAttachment item, string user);
        Response<bool> UpdateField(string rfa_key, string fieldName, string fieldValue, string user);
        void Attach_RfpRequest_File(string rfa_key, string rfp_key);
    }
}
