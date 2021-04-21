using ICMA.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ICMA.Filters
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedRoles;
        public RoleAuthorizeAttribute(string roles)
        {
            if (!string.IsNullOrWhiteSpace(roles))
                this.allowedRoles = roles.Split(',');
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (SessionHelper.CurrentUser == null) return false;
            return allowedRoles.Any(p=> p.Equals(SessionHelper.CurrentUser.User.per_type, StringComparison.OrdinalIgnoreCase));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (SessionHelper.CurrentUser == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login?returnUrl=" + HttpContext.Current.Request.Url.PathAndQuery);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Unauthorized" }));//new HttpUnauthorizedResult();
            }
        }
    }
}