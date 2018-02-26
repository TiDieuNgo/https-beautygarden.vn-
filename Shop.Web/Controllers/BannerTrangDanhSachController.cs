using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Repositories;
using Shop.Model;

namespace Shop.Web.Controllers
{
    public class BannerTrangDanhSachController : Controller
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IBannerDanhSachRepository _bannerDanhSachRepository;

        public BannerTrangDanhSachController(IMenuRepository menuRepository, IBannerDanhSachRepository bannerDanhSachRepository)
        {
            _menuRepository = menuRepository;
            _bannerDanhSachRepository = bannerDanhSachRepository;
        }
        public ActionResult Index(int iddanhmuc)
        {
            IList<BannerDanhSach> banner = _bannerDanhSachRepository.GetMany(o=>o.IdDanhMuc==iddanhmuc&&o.Ok).OrderBy(o => o.ViTri).ToList();
            return View("BannerDanhSach",banner);
       }
    
    }
}
