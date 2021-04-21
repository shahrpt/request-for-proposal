using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Infrastructure.Core;

namespace UrbanRFP.Facade.Interfaces
{
    public interface IAuthFacade 
    {
        Response<CurrentUser> Authenticate(string userName, string password);

        CurrentUser GetCurrentUser(string UserID);
    }
}
