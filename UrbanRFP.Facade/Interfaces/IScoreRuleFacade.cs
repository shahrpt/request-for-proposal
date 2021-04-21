using System.Collections.Generic;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Facade.Interfaces
{
    public interface IScoreRuleFacade
    {
        Response<RfpScoreRule> Add(RfpScoreRule item, string user);
        Response<RfpScoreRule> Update(RfpScoreRule item, string user);
        List<RfpScoreRule> GetWhere(string search);
        bool Remove(string id, string user);
    }
}
