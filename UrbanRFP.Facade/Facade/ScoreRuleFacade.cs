using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Facade.Facade
{
    public class ScoreRuleFacade : IScoreRuleFacade
    {
        private readonly IScoreRuleRepository _scoreRuleRepositoryDAC;

        #region CONSTRUCTORS
        public ScoreRuleFacade(IScoreRuleRepository scoreRuleRepositoryDAC)
        {
            _scoreRuleRepositoryDAC = scoreRuleRepositoryDAC ?? throw new ArgumentNullException(nameof(scoreRuleRepositoryDAC));
        }

        #endregion

        public List<RfpScoreRule> GetWhere(string search)
        {
            return _scoreRuleRepositoryDAC.GetWhere(search);
        }

        public Response<RfpScoreRule> Add(RfpScoreRule item, string user)
        {
            return _scoreRuleRepositoryDAC.Add(item, user);
        }

        public Response<RfpScoreRule> Update(RfpScoreRule item, string user)
        {
            return _scoreRuleRepositoryDAC.Update(item, user);
        }

        public bool Remove(string id, string user)
        {
            return _scoreRuleRepositoryDAC.Remove(id, user);
        }
    }
}
