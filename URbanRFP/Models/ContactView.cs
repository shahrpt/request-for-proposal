using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICMA.Models
{
    public class ContactView
    {
        public string full_name{ get; set; }

        public string email { get; set; }

        public string title { get; set; }

        public string role { get; set; }

        public string role_id { get; set; }
    }
}