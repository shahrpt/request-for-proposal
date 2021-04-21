using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanRFP.Infrastructure.Entity
{
    public class SearchFilter
    {
        public string Text { get; set; }

        public string Types { get; set; }

        public string Sub_Types { get; set; }

        public string OrgKey { get; set; }

        public string OrgKey_search { get; set; }
    }
}
