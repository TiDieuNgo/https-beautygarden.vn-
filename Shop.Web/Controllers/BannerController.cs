using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Repositories;
using Shop.Model;


namespace Shop.Web.Controllers
{
    public class BannerController : Controller
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IBannerDanhSachRepository _bannerDanhSachRepository;
        public BannerController(IMenuRepository menuRepository, IBannerDanhSachRepository bannerDanhSachRepository)
        {
            _menuRepository = menuRepository;
            _bannerDanhSachRepository = bannerDanhSachRepository;
        }

        public ActionResult Index()
        {
            var banners = _menuRepository.GetBannersHome();
            return View("BannerPartial", banners);
       }
        public ActionResult BannerQCDanhSach()
        {
            IList<BannerDanhSach> banner = _bannerDanhSachRepository.GetMany(o => o.BannerQCDanhSach&&o.Ok).ToList();
            return View("BannerQCDanhSach", banner);
        }
        public ActionResult BannerQCChiTiet()
        {
            IList<BannerDanhSach> banner = _bannerDanhSachRepository.GetMany(o => o.BannerQCChiTiet&&o.Ok).ToList();
            return View("BannerQCChiTiet", banner);
        }
    
    }
}
