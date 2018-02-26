using System.Web.Mvc;


namespace Shop.Web.Controllers
{
    public class GiaoDienMoiController : Controller
    {
       public ActionResult Index()
       {
           return View("giaodienmoi");
       }
    
    }
}
