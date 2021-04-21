using System.Collections.Generic;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Infrastructure.Interfaces
{
    public interface IRfpAttachmentRepository : IRepository<RfpAttachment>
    {
        List<RfpAttachment> GetWhere(string whereClause);
        Response<bool> UpdateField(string rfa_key, string fieldName, string fieldValue, string user);
        void Attach_RfpRequest_File(string rfa_key, string rfp_key);
    }
}
