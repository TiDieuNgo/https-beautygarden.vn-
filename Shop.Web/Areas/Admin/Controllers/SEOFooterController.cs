using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class SEOFooterController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISEOFooterRepository _seoFooterRepository;
        private readonly IMenuRepository _menuRepository;

        public SEOFooterController(IUnitOfWork unitOfWork, ISEOFooterRepository seoFooterRepository, IMenuRepository menuRepository)
        {
            _unitOfWork = unitOfWork;
            _seoFooterRepository = seoFooterRepository;
            _menuRepository = menuRepository;
        }

        public ActionResult Index()
        {
            var dl = _seoFooterRepository.GetAll();
            return View(dl);
        }

        public ActionResult Edit(int id)
        {
            var dlcu = _seoFooterRepository.GetById(id);
            return View("Edit", dlcu);
        }
        public ActionResult Add()
        {
            return View("Create");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(SEOFooter model)
        {
            SEOFooter seoFooter = new SEOFooter()
                                  {
                                      Link = model.Link,
                                      TieuDe = model.TieuDe,
                                      NgayTao = DateTime.Now,
                                      Showhide = false
                                  };
            _seoFooterRepository.Add(seoFooter);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(SEOFooter model)
        {
            SEOFooter dlcu = _seoFooterRepository.GetById(model.Id);
            dlcu.Id = model.Id;
            dlcu.TieuDe = model.TieuDe;
            dlcu.NgayTao = DateTime.Now;
            dlcu.Showhide = true;
            dlcu.Link = model.Link;
            _seoFooterRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");

        }
        public ActionResult delete(int id)
        {
            var idcu = _seoFooterRepository.GetById(id);
            _seoFooterRepository.Delete(idcu);
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
                    context.Database.ExecuteSqlCommand("update SEOFooters set Showhide='True' where id_ ={0}", idsanpham);

                }
            }
            else
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update SEOFooters set Showhide='False' where id_ ={0}", idsanpham);

                }
            }
            _menuRepository.ClearCacheByKey(new List<string>() { Shop.Web.Core.Cache.CacheKey.AllMenu });
            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }
    }
}
