using System;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Helper;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class PopUpBannerKMController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPopUpBannerKMRepository _popUpBannerKmRepository;

        public PopUpBannerKMController(IUnitOfWork unitOfWork, IPopUpBannerKMRepository popUpBannerKmRepository)
        {
            _unitOfWork = unitOfWork;
            _popUpBannerKmRepository = popUpBannerKmRepository;
        }

        public ActionResult Index()
        {
            var dl = _popUpBannerKmRepository.GetAll();
            return View(dl);
        }
        public ActionResult Edit(int id)
        {
            var dlcu = _popUpBannerKmRepository.GetById(id);
            return View("Edit", dlcu);
        }
        public ActionResult Add()
        {
            return View("Create");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(PopUpBannerKMModel model)
        {
            DateTime start = DateTimeHelper.ConvertToLongDay(model.StartDay);
            DateTime end = DateTimeHelper.ConvertToLongDay(model.EndDay);
            if (ModelState.IsValid)
            {
                PopUpBannerKM menu = new PopUpBannerKM()
                {
                    HinhAnh = (model.HinhAnh).Replace("/files/", ""),
                    TieuDe = model.TieuDe,
                    Created = DateTime.Now,
                    Link = model.Link,
                    EndDay = end,
                    Active = true,
                    StartDay = start
                };
                _popUpBannerKmRepository.Add(menu);
                _unitOfWork.Commit();
            }
            else
            {
                return View("Create", model);
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(PopUpBannerKMModel model)
        {
            DateTime start = DateTimeHelper.ConvertToLongDay(model.StartDay);
            DateTime end = DateTimeHelper.ConvertToLongDay(model.EndDay);
            PopUpBannerKM dlcu = _popUpBannerKmRepository.GetById(model.Id);
            dlcu.Id = model.Id;
            dlcu.TieuDe = model.TieuDe;
            dlcu.Created = DateTime.Now;
            dlcu.Link = model.Link;
            dlcu.StartDay = start;
            dlcu.EndDay = end;
            dlcu.HinhAnh = (model.HinhAnh).Replace("/files/", "");
            _popUpBannerKmRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult delete(int id)
        {
            var idcu = _popUpBannerKmRepository.GetById(id);
            _popUpBannerKmRepository.Delete(idcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
