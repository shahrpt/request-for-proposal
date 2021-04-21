using System.Collections.Generic;

namespace UrbanRFP.Domain.ViewModel
{
    public class SubMenuViewModel
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }
        public string MenuIconAlt { get; set; }
        public int? MenuParentID { get; set; }
        public string MenuLink { get; set; }
        public int MenuSeq { get; set; }
    }
}
