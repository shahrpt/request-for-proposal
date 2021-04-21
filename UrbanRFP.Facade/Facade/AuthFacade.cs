using System;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Helpers;
using UrbanRFP.Infrastructure.Interfaces;

namespace UrbanRFP.Facade.Facade
{
    public class AuthFacade : IAuthFacade
    {
        private readonly IContactRepository _contactRepositoryDAC;

        #region CONSTRUCTORS
        public AuthFacade(IContactRepository contactRepositoryDAC)
        {
            _contactRepositoryDAC = contactRepositoryDAC ?? throw new ArgumentNullException(nameof(contactRepositoryDAC));
        }

        #endregion

        public Response<CurrentUser> Authenticate(string userName, string password)
        {
            Response<UserProfile> _user = _contactRepositoryDAC.GetByUsername(userName);
            Response<CurrentUser> response = new Response<CurrentUser>(new CurrentUser(_user.Result));

            if (_user.Success && _user.Result.ct_active > 0)
            {
                if (_user.Result.ct_password.Equals(password.GetEncryptedString()))
                {
                    response.Status = Status.OK;
                }
                else
                {
                    response.Status = Status.NotAuthenticted;
                }
            }
            else
            {
                response.Status = Status.NotAuthenticted;
            }

            return response;
        }

        public CurrentUser GetCurrentUser(string UserID)
        {
            var resp = _contactRepositoryDAC.GetByKey(UserID);
            CurrentUser _currentUser = new CurrentUser(resp.Result);

            if (!resp.Success)
            {
                _currentUser = null;
            }

            return _currentUser;
        }
    }
}
