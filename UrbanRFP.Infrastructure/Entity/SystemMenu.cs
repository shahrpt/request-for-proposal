using System.Collections.Generic;
using UrbanRFP.Domain.ViewModel;

namespace UrbanRFP.Infrastructure.Entity
{
    public class SystemMenu
    {

        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }
        public string MenuIconAlt { get; set; }
        public string Notes { get; set; }
        public int? MenuParentID { get; set; }
        public bool IsActive { get; set; }
        public string MenuLink { get; set; }
        public int MenuSeq { get; set; }
        public List<MenuViewModel> SubMenu { get; set; }
    }
}
