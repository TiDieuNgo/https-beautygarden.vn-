using System.Web.Mvc;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class DanhSachTagController : Controller
    {
        public ActionResult TagSanPham()
        {
            return View("CreateTagSanPham");
        }
        public ActionResult TagSuKienKM()
        {
            return View("TagSuKienKM");
        }
        public ActionResult TagBiQuyetLamDep()
        {
            return View("TagBiQuyetLamDep");
        }
        public ActionResult TagReview()
        {
            return View("TagReview");
        }
    }
}
