using ICMA.Controllers;
using ICMA.Filters;
using System.Web.Mvc;

namespace UrbanRFP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}