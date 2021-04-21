using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanRFP.Infrastructure.Entity;

namespace UrbanRFP.Domain.ViewModel
{
    public class CurrentUser
    {
        private CurrentUser()
        {
            // No argument constructor.
        }

        public CurrentUser(UserProfile _user)
        {
            this.User = _user;
        }

        public UserProfile User { get; set; }
    }
}
