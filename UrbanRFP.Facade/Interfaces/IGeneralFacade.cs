using System.Collections.Generic;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Facade.Interfaces
{
    public interface IGeneralFacade
    {
        //List<SystemMenu> GetMenuItems();

        List<MenuViewModel> GetMenuItems();
    }
}
