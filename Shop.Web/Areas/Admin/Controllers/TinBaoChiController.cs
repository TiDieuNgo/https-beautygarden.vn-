using System;
using System.Web;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Models;

namespace Shop.Admin.Controllers
{
     [ShopAuthorize]
    public class TinBaoChiController : BaseController
    {
        private readonly IDetailMenuRepository _detailMenuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;
        private readonly HttpContextBase _httpContext;
        public TinBaoChiController(IDetailMenuRepository detailMenuRepository, IUnitOfWork unitOfWork, HttpContextBase httpContext)
        {
            _detailMenuRepository = detailMenuRepository;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }

        public ActionResult add()
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Tintuc_Create)))
                return View("_NoAuthor");
            return View("Create");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(DetailMenu model)
        {
            model.Name = model.Name;
            model.Img = model.Img;
            model.Link = model.Link;
            model.id_Menu = 1462;
            model.Ma_Hang = "";
            model.NameAdwords = "";
            model.ok = true;
            model.Number = 0;
            model.ShowMenu = true;
            model.ShowKhuyenMai = false;
            model.ShowIconHot = false;
            model.ShowIconNew = false;
            model.TinhTrangSP = false;
            model.idUser = 15;
            model.idUserOk = 15;
            model.sDate = DateTime.Now;
            model.sDateOk = DateTime.Now;
            model.SEODescription = model.SEODescription;
            _detailMenuRepository.Add(model);
            _unitOfWork.Commit();
            return RedirectToAction("TinBaoChi", "TinTuc");

        }

        public ActionResult Edit(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Tintuc_Edit)))
                return View("_NoAuthor");
            var dlcu = _detailMenuRepository.GetById(id);
            return View("Edit", dlcu);

        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(DetailMenu model)
        {
            DetailMenu dlcu = _detailMenuRepository.GetById(model.id_);
            dlcu.id_ = model.id_;
            dlcu.Name = model.Name;
            dlcu.Img = model.Img;
            dlcu.Link = model.Link;
            dlcu.id_ = model.id_;
            dlcu.SEODescription = model.SEODescription;
            dlcu.SEOtitle = model.SEOtitle;
            _detailMenuRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("TinBaoChi", "TinTuc"); 
        }
        public ActionResult delete(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Tintuc_Delete)))
                return View("_NoAuthor");

            var idxoa = _detailMenuRepository.GetById(id);
            _detailMenuRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("TinBaoChi", "TinTuc");
        }
    }
}
