using System.Collections.Generic;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Models;

namespace ICMA.Models
{
    public class RACIViewModel
    {
        public RACIViewModel()
        {
            NewContact = new RegisterViewModel();
            Contacts = new List<UserProfile>();
            Organization = new CoOrganization();
        }

        public RegisterViewModel NewContact { get; set; }
        public List<UserProfile> Contacts { get; set; }
        public CoOrganization Organization { get; set; }
    }
}