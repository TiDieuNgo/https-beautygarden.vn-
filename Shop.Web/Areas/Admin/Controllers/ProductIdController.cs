using System.Web.Mvc;
using Shop.Data.Repositories;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
     [ShopAuthorize]
    public class ProductIdController : BaseController
    {
        private readonly IMenuRepository _menuRepository;

        public ProductIdController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public ActionResult Index(int id)
        {
            string linksp = _menuRepository.GetLinkById(id);
            return RedirectToRoute("ChiTietSanPham", new { splink = linksp });
        }

      
    }
}
