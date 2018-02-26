using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
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
    public class SalePageController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISalePageRepository _salePageRepository;
        private const int pageSize = 20;
        public SalePageController(IUnitOfWork unitOfWork, ISalePageRepository salePageRepository)
        {
            _unitOfWork = unitOfWork;
            _salePageRepository = salePageRepository;
        }

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<SalePage> detail = _salePageRepository.GetAll().OrderByDescending(o => o.NgayTao).ToList();
            var data = detail.ToPagedList(pageNumber, pageSize);
            return View("Index",data);
        }

        public ActionResult Edit(int id)
        {
            var dlcu = _salePageRepository.GetById(id);
            return View("Edit", dlcu);
        }
        public ActionResult Add()
        {
            return View("Create");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(SalePageModel model)
        {
            if (ModelState.IsValid)
            {
                SalePage menu = new SalePage()
                {
                    Link = ConvertFont(model.Link),
                    CodeName = RejectMarks(model.TieuDe),
                    KeyWord = model.KeyWord,
                    NgayTao = DateTime.Now,
                    NoiDung1 = model.NoiDung1,
                    NoiDung2 = model.NoiDung2,
                    NoiDung3 = model.NoiDung3,
                    NoiDung4 = model.NoiDung4,
                    SDT = model.SDT,
                    SeoDecription = model.SeoDecription,
                    SeoTitle = model.SeoTitle,
                    TieuDe = model.TieuDe,
                    HinhAnh = model.HinhAnh,
                    Showhide = false
                };
                _salePageRepository.Add(menu);
                _unitOfWork.Commit();
            }
            else
            {
                return View("Create", model);
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(SalePage model)
        {
            SalePage dlcu = _salePageRepository.GetById(model.Id);
            dlcu.Id = model.Id;
            dlcu.Link = ConvertFont(model.Link);
            dlcu.TieuDe = model.TieuDe;
            dlcu.CodeName = RejectMarks(model.TieuDe);
            dlcu.NgayTao = DateTime.Now;
            dlcu.SDT = model.SDT;
            dlcu.SeoTitle = model.SeoTitle;
            dlcu.SeoDecription = model.SeoDecription;
            dlcu.KeyWord = model.KeyWord;
            dlcu.NoiDung1 = model.NoiDung1;
            dlcu.NoiDung2 = model.NoiDung2;
            dlcu.NoiDung3 = model.NoiDung3;
            dlcu.NoiDung4 = model.NoiDung4;
            dlcu.HinhAnh = model.HinhAnh;
            dlcu.Showhide = model.Showhide;
            _salePageRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult delete(int id)
        {
            var idcu = _salePageRepository.GetById(id);
            _salePageRepository.Delete(idcu);
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
                    context.Database.ExecuteSqlCommand("update SalePages set Showhide='True' where Id ={0}", idsanpham);
                }
            }
            else
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update SalePages set Showhide='False' where Id ={0}", idsanpham);
                }
            }
            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }
    }
}
