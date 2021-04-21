using System.Collections.Generic;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Infrastructure.Interfaces
{
    public interface IGeneralRepository
    {
        List<SystemMenu> GetMenuItems();
    }
}
