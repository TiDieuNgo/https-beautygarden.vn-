using System.Web.Mvc;


namespace Shop.Web.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult PageNotFound()
        {
            //loi khong tim thay trang
            Response.StatusCode = 404;
            return View("404erro");
        }

        public ActionResult Forbidden()
        {
            //loi khong co quyen truy cap hoac request yeu
            Response.StatusCode = 403;
            return View("403erro");
        }
        public ActionResult ServerError()
        {
            //sự cố nào đó xuất hiện trên server trang web bạn truy cập
            Response.StatusCode = 500;
            return View("500erro");
        }
    }
}
