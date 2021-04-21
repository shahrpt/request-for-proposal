using System;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Facade.Facade
{
    public class MiscFacade : IMiscFacade
    {
        private readonly IMiscRepository _miscRepositoryDAC;

        #region CONSTRUCTORS
        public MiscFacade(IMiscRepository miscRepositoryDAC)
        {
            _miscRepositoryDAC = miscRepositoryDAC ?? throw new ArgumentNullException(nameof(miscRepositoryDAC));
        }

        #endregion

        public Response<int> MarkFavorite(CoFavorite item)
        {
            return _miscRepositoryDAC.MarkFavorite(item);
        }

        public Response<int> ViewPage(CoPageView item)
        {
            return _miscRepositoryDAC.ViewPage(item);
        }
    }
}
