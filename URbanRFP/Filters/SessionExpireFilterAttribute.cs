using ICMA.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ICMA.Filters
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            // check if session is supported
            if (SessionHelper.CurrentUser == null || ctx == null)
            {
                // check if a new session id was generated
                filterContext.Result = new RedirectResult("~/Account/Login?returnUrl=" + HttpContext.Current.Request.Url.PathAndQuery);
                /*filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Account" },
                    { "action", "Login" }
                });*/
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}