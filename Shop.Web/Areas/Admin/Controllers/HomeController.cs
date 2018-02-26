using System.Web;
using System.Web.Mvc;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Extensions;
using Shop.Web.Core.Models;

namespace Shop.Admin.Controllers
{
     [ShopAuthorize]
    public class HomeController : BaseController
    {
         private readonly HttpContextBase _httpContext;

         public HomeController(HttpContextBase httpContext)
         {
             _httpContext = httpContext;
         }

         public ActionResult Index()
        {
            ShopUser efmvcUser = _httpContext.User.GetShopUser();
            return View("Index");
        }
       
      
    }
}
