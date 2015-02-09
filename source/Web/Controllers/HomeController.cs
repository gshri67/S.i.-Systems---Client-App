using System.Web.Mvc;

namespace SiSystems.ClientApp.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AuthTest()
        {
            if(HttpContext.IsDebuggingEnabled)
                return View();
    
            return RedirectToAction("Index");
        }
    }
}
