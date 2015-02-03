using System.Web.Mvc;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }
    }
}
