using System.Web.Mvc;
using Shop.Web.Core.Models;
using Shop.Web.Core.Extensions;
namespace Shop.Web.Core.ActionFilters
{

    //Inject a ViewBag object to Views for getting information about an authenticated user
    public class UserFilter : ActionFilterAttribute
    {
        
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            UserModel userModel;
            if (filterContext.Controller.ViewBag.UserModel == null)
            {
                userModel = new UserModel();
                filterContext.Controller.ViewBag.UserModel = userModel;
            }
            else
            {
                userModel = filterContext.Controller.ViewBag.UserModel as UserModel;
            }
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ShopUser ShopUser = filterContext.HttpContext.User.GetShopUser();
                userModel.IsUserAuthenticated = ShopUser.IsAuthenticated;
                userModel.UserName = ShopUser.DisplayName;
                userModel.RoleName = ShopUser.RoleName;
            }

            base.OnActionExecuted(filterContext);
        }
    }
    public class UserModel
    {
        public bool IsUserAuthenticated { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }          
    }
}
