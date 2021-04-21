using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;
using UrbanRPF.Mapping;

namespace UrbanRFP.Facade.Facade
{
    public class GeneralFacade : IGeneralFacade
    {
        private readonly IGeneralRepository _generalRepositoryDAC;

        #region CONSTRUCTORS
        public GeneralFacade(IGeneralRepository generalRepositoryDAC)
        {
            _generalRepositoryDAC = generalRepositoryDAC ?? throw new ArgumentNullException(nameof(generalRepositoryDAC));
        }

        public List<MenuViewModel> GetMenuItems() => AutoMapperHelper<SystemMenu, MenuViewModel>.MapList(_generalRepositoryDAC.GetMenuItems()).ToList();

        //public List<SystemMenu> GetMenuItems()
        //{
        //    return _generalRepositoryDAC.GetMenuItems();
        //}

        #endregion
    }
}
