using System;
using System.Collections.Generic;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Facade.Facade
{
    public class RfpAttachmentFacade : IRfpAttachmentFacade
    {
        private readonly IRfpAttachmentRepository _rfpAttachmentRepositoryDAC;

        #region CONSTRUCTORS
        public RfpAttachmentFacade(IRfpAttachmentRepository rfpAttachmentRepositoryDAC)
        {
            _rfpAttachmentRepositoryDAC = rfpAttachmentRepositoryDAC ?? throw new ArgumentNullException(nameof(rfpAttachmentRepositoryDAC));
        }

        #endregion

        public Response<RfpAttachment> Add(RfpAttachment item, string user)
        {
            return _rfpAttachmentRepositoryDAC.Add(item, user);
        }

        public Response<RfpAttachment> Find(string id)
        {
            return _rfpAttachmentRepositoryDAC.Find(id);
        }

        public List<RfpAttachment> GetAll()
        {
            return _rfpAttachmentRepositoryDAC.GetAll();
        }

        public List<RfpAttachment> GetWhere(string whereClause)
        {
            return _rfpAttachmentRepositoryDAC.GetWhere(whereClause);
        }

        public bool Remove(string id, string user)
        {
            return _rfpAttachmentRepositoryDAC.Remove(id, user);
        }

        public Response<RfpAttachment> Update(RfpAttachment item, string user)
        {
            return _rfpAttachmentRepositoryDAC.Update(item, user);
        }

        public Response<bool> UpdateField(string rfa_key, string fieldName, string fieldValue, string user)
        {
            return _rfpAttachmentRepositoryDAC.UpdateField(rfa_key, fieldName, fieldValue, user);
        }

        public void Attach_RfpRequest_File(string rfa_key, string rfp_key)
        {
            _rfpAttachmentRepositoryDAC.Attach_RfpRequest_File(rfa_key, rfp_key);
        }
    }
}