using System.Collections.Generic;
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
    public class BannerTrangDanhSachController : BaseController
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBannerDanhSachRepository _bannerDanhSachRepository;
        private readonly HttpContextBase _httpContext;
        public BannerTrangDanhSachController(IMenuRepository menuRepository, IUnitOfWork unitOfWork, IBannerDanhSachRepository bannerDanhSachRepository, HttpContextBase httpContext)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
            _bannerDanhSachRepository = bannerDanhSachRepository;
            _httpContext = httpContext;
        }

        public ActionResult Index(int? sx)
        {
          
            if (!sx.HasValue)
            {
                var dl = _bannerDanhSachRepository.GetMany(o=>o.IdDanhMuc!=0).ToList();
                ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                return View("Index", dl);
            }
            else
            {
                if (sx.Value == 1)
                {
                    //trang diem
                      var banner = _bannerDanhSachRepository.GetMany(o => o.IdDanhMuc == 2).ToList();
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", banner);
                }
                else if (sx.Value==2)
                {
                    //cham soc da
                    var banner = _bannerDanhSachRepository.GetMany(o => o.IdDanhMuc == 3).ToList();
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", banner);
                }
                else if (sx.Value == 3)
                {
                    //cham soc da
                    var banner = _bannerDanhSachRepository.GetMany(o => o.IdDanhMuc == 5).ToList();
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", banner);
                }
                else if (sx.Value == 4)
                {
                    //cham soc da
                    var banner = _bannerDanhSachRepository.GetMany(o => o.IdDanhMuc == 7).ToList();
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", banner);
                }
                else if (sx.Value == 5)
                {
                    //cham soc da
                    var banner = _bannerDanhSachRepository.GetMany(o => o.IdDanhMuc == 8).ToList();
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", banner);
                }
                else if (sx.Value == 6)
                {
                    //cham soc da
                    var banner = _bannerDanhSachRepository.GetMany(o => o.IdDanhMuc == 1238).ToList();
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", banner);
                }
                else
                {
                    //cham soc da
                    var banner = _bannerDanhSachRepository.GetMany(o => o.IdDanhMuc == 1240).ToList();
                    ViewData["sx"] = sx.HasValue ? sx.Value : 1;
                    return View("Index", banner);
                }
            }
          

        }

        public ActionResult Edit(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Banner_Edit)))
                return View("_NoAuthor");

            var dlcu = _bannerDanhSachRepository.GetById(id);
            var menu = _menuRepository.GetMany(o => o.Style == "danh-muc-san-pham" && o.idControl == 1 && o.ok).OrderBy(o => o.sPosition);
            IList<SelectListItem> danhsachMenu = new List<SelectListItem>();
            if (menu.Any())
            {
                foreach (var ncc in menu)
                {
                    danhsachMenu.Add(new SelectListItem()
                    {
                        Text = ncc.NameProduct,
                        Value = ncc.id_.ToString()
                    });
                }
            }
            ViewData["danhmuccap1"] = danhsachMenu;
          
            return View("Edit", dlcu);

        }
        public ActionResult Add()
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Banner_Create)))
                return View("_NoAuthor");

            var menu = _menuRepository.GetMany(o => o.Style == "danh-muc-san-pham" && o.idControl == 1 && o.ok).OrderBy(o => o.sPosition);
            IList<SelectListItem> danhsachMenu = new List<SelectListItem>();
            if (menu.Any())
            {
                foreach (var ncc in menu)
                {
                    danhsachMenu.Add(new SelectListItem()
                    {
                        Text = ncc.NameProduct,
                        Value = ncc.id_.ToString()
                    });
                }
            }
            ViewData["danhmuccap1"] = danhsachMenu;
            return View("Create", new BannerTrangDanhSachModel());
            
        }
        //them moi tu danh sach tag ngoai menu admin
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(BannerTrangDanhSachModel model)
        {
             if (ModelState.IsValid)
            {
                BannerDanhSach menu = new BannerDanhSach()
                {
                    HinhAnh = (model.HinhAnh).Replace("/files/", ""),
                    TenBanner=model.TenBanner,
                    ViTri = model.ViTri,
                    IdDanhMuc = model.IdDanhMuc,
                    BannerQCChiTiet = false,
                    BannerQCDanhSach = false,
                    LinkHttp1 = model.LinkHttp1,
                    Ok = false
                };
            _bannerDanhSachRepository.Add(menu);
            _unitOfWork.Commit();
            }
             else
             {
                 return View("Create", model);
             }
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(BannerDanhSach model)
        {
            BannerDanhSach dlcu = _bannerDanhSachRepository.GetById(model.id_);
            dlcu.id_ = model.id_;
            dlcu.TenBanner = model.TenBanner;
            dlcu.ViTri = model.ViTri;
            dlcu.HinhAnh = (model.HinhAnh).Replace("/files/", "");
            dlcu.IdDanhMuc = model.IdDanhMuc;
            dlcu.BannerQCChiTiet = false;
            dlcu.BannerQCDanhSach = false;
            dlcu.LinkHttp1 = model.LinkHttp1;
            dlcu.Ok = model.Ok;
            _bannerDanhSachRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult delete(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Banner_Delete)))
                return View("_NoAuthor");

            var idcu = _bannerDanhSachRepository.GetById(id);
            _bannerDanhSachRepository.Delete(idcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult UpdateOk(int idsanpham, bool Checked)
        {
            if (Checked)
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update BannerDanhSachs set Ok='True' where id_ ={0}", idsanpham);

                }
            }
            else
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update BannerDanhSachs set Ok='False' where id_ ={0}", idsanpham);

                }
            }

            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }
    }
}
