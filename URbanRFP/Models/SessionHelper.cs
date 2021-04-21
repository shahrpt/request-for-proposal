using SimpleInjector;
using System.Web;
using System.Web.Security;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Facade.Facade;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;

namespace ICMA.Models
{
    public class SessionHelper
    {
        static readonly Container container;
        private static readonly IAuthFacade authService;

        static SessionHelper()
        {
            container = UrbanRFP.SimpleInjectorConfig.container;
            authService = container.GetInstance<AuthFacade>();
        }

        public static T Read<T>(string variable)
        {
            object value = null;
            if (HttpContext.Current != null) value = HttpContext.Current.Session[variable];

            if (value == null)
                return default(T);
            else
                return ((T)value);
        }

        public static void Write(string variable, object value)
        {
            HttpContext.Current.Session[variable] = value;
        }

        public static CurrentUser CurrentUser
        {
            get
            {
                CurrentUser _userInfo = Read<CurrentUser>(Constants.CURRENT_USER);

                if (_userInfo == null && HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                    _userInfo = authService.GetCurrentUser(ticket.UserData);

                    if (_userInfo != null) Write(Constants.CURRENT_USER, _userInfo);
                }

                return _userInfo;
            }
            set { Write(Constants.CURRENT_USER, value); }
        }
    }
}