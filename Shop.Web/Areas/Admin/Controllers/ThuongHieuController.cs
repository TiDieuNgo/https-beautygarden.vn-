using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
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
    public class ThuongHieuController : BaseController
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;
        private readonly HttpContextBase _httpContext;
        public ThuongHieuController(IMenuRepository menuRepository, IUnitOfWork unitOfWork, HttpContextBase httpContext)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<Menu> tags = _menuRepository.GetMany(o => o.Style == "thuong-hieu").OrderBy(o => o.sPosition).ToList();
            var data = tags.ToPagedList(pageNumber, pageSize);
            return View("Index", data);

        }
        public ActionResult Add()
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Thuong_hieu_Create)))
                return View("_NoAuthor");

            return View("Create");
        }
        //them moi
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(ThemThuongHieuModel objectMenu)
        {

            if (ModelState.IsValid)
            {
                Menu model = new Menu()
                {
                    CodeProduct = RejectMarks(objectMenu.NameProduct),
                    Img = (objectMenu.Img).Replace("/files/", ""),
                    IdNhaCungCap = 12,
                    idControl = 10,
                    ok = true,
                    Link = ConvertFont(objectMenu.NameProduct),
                    LevelMenu = 1,
                    LinkHttp = "https://beautygarden.vn/" + ConvertFont(objectMenu.NameProduct) + ".html",
                    Style = "thuong-hieu",
                    ui = "vi",
                    sPosition = objectMenu.sPosition,
                    sDate = DateTime.Now,
                    sDateOk = DateTime.Now,
                    idUser = 18,
                    idUserOk = 18,
                    NameProduct = objectMenu.NameProduct,
                    Note = objectMenu.Note,
                    //MenuAdwords = (objectMenu.MenuAdwords).Replace("/files/", ""),
                    LinkHttp1 = objectMenu.LinkHttp1,
                    SEODescription = objectMenu.SEODescription,
                    SEOtitle = objectMenu.SEOtitle
                };
                _menuRepository.Add(model);
                _menuRepository.ClearCacheByKey(new List<string>() { Shop.Web.Core.Cache.CacheKey.AllBrandCacheKey });
                _unitOfWork.Commit();
            }
            else
            {
                return View("Create", objectMenu);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Thuong_hieu_Edit)))
                return View("_NoAuthor");
            var idcu = _menuRepository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(Menu model)
        {
            Menu menu = _menuRepository.GetById(model.id_);
            menu.id_ = model.id_;
            menu.NameProduct = model.NameProduct;
            menu.Img = (model.Img).Replace("/files/", "");
            menu.sPosition = model.sPosition;
            menu.Note = model.Note;
            //menu.MenuAdwords = (model.MenuAdwords).Replace("/files/", "");
            menu.LinkHttp1 = model.LinkHttp1;
            menu.SEODescription = model.SEODescription;
            menu.SEOtitle = model.SEOtitle;
            _menuRepository.Update(menu);
            _menuRepository.ClearCacheByKey(new List<string>() { Shop.Web.Core.Cache.CacheKey.AllBrandCacheKey });
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult delete(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Thuong_hieu_Delete)))
                return View("_NoAuthor");

            var idxoa = _menuRepository.GetById(id);
            _menuRepository.Delete(idxoa);
            _menuRepository.ClearCacheByKey(new List<string>() { Shop.Web.Core.Cache.CacheKey.AllBrandCacheKey });
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

            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }
       
    }
}
