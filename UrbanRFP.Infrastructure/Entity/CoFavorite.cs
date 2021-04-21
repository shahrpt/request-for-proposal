using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanRFP.Infrastructure.Entity
{
    public class CoFavorite
    {
        public Guid fav_key { get; set; }
        public Guid fav_org_key { get; set; }
        public Guid? fav_prd_key { get; set; }
        public Guid? fav_rfp_key { get; set; }
        public DateTime? fav_add_date { get; set; }
        public string fav_add_user { get; set; }
        public DateTime? fav_change_date { get; set; }
        public string fav_change_user { get; set; }
        public bool is_favorite { get; set; }
    }
}
