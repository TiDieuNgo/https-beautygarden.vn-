using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Models;

namespace Shop.Admin.Controllers
{
     [ShopAuthorize]
    public class BannerTrangChuController : BaseController
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpContextBase _httpContext;

        public BannerTrangChuController(IMenuRepository menuRepository, IUnitOfWork unitOfWork, HttpContextBase httpContext)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }
        public ActionResult Index()
        {
            var dl =
                _menuRepository.GetMany(o => o.idControl == 23 && o.Style == "banner-website-top-desktop").OrderByDescending(o => o.sDate);

            return View(dl);
        }

        public ActionResult Edit(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Banner_Edit)))
                return View("_NoAuthor");
            var dlcu = _menuRepository.GetById(id);
            return View("Edit", dlcu);

        }
        public ActionResult Add()
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Banner_Create)))
                return View("_NoAuthor");
            return View("Create");
        }
        //them moi tu danh sach tag ngoai menu admin
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(ThemBannerTrangChuModel model)
        {
            if (ModelState.IsValid)
            {
                Menu menu = new Menu()
                {
                    CodeProduct = RejectMarks(model.NameProduct),
                    Img = (model.Img).Replace("/files/", ""),
                    IdNhaCungCap = 12,
                    idControl = 23,
                    ok = true,
                    Link = ConvertFont(model.NameProduct),
                    LevelMenu = 1,
                    LinkHttp = "https://beautygarden.vn/" + ConvertFont(model.NameProduct) + ".html",
                    Style = "banner-website-top-desktop",
                    sPosition = model.sPosition,
                    sDate = DateTime.Now,
                    sDateOk = DateTime.Now,
                    idUser = 18,
                    idUserOk = 18,
                    NameProduct = model.NameProduct,
                    LinkHttp1 = model.LinkHttp1,
                    NgayHetHang = DateTime.Now
                };
                _menuRepository.Add(menu);
                _menuRepository.ClearCacheBannerHome( Shop.Web.Core.Cache.CacheKey.BannerHomeCacheKey );
                _unitOfWork.Commit();
            }
            else
            {
                return View("Create", model);
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(Menu model)
        {
            Menu dlcu = _menuRepository.GetById(model.id_);
            dlcu.id_ = model.id_;
            dlcu.NameProduct = model.NameProduct;
            dlcu.Img = model.Img;
            dlcu.sPosition = model.sPosition;
            dlcu.sDate = DateTime.Now;
            dlcu.sDateOk = DateTime.Now;
            dlcu.Img = (model.Img).Replace("/files/", "");
            dlcu.LinkHttp1 = model.LinkHttp1;
            dlcu.NgayHetHang = DateTime.Now;
            _menuRepository.Update(dlcu);
            _unitOfWork.Commit();
            _menuRepository.ClearCacheBannerHome(Shop.Web.Core.Cache.CacheKey.BannerHomeCacheKey);
            return RedirectToAction("Index");

        }
        public ActionResult delete(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Banner_Delete)))
                return View("_NoAuthor");

            var idcu = _menuRepository.GetById(id);
            _menuRepository.Delete(idcu);
            _unitOfWork.Commit();
            _menuRepository.ClearCacheBannerHome(Shop.Web.Core.Cache.CacheKey.BannerHomeCacheKey);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult UpdateOk(int idsanpham, bool Checked)
        {
            if (Checked)
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set ok='True' where id_ ={0}", idsanpham);
                   
                }
            }
            else
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set ok='False' where id_ ={0}", idsanpham);
                  
                }
            }
            _menuRepository.ClearCacheBannerHome(Shop.Web.Core.Cache.CacheKey.BannerHomeCacheKey);
            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }
    }
}
