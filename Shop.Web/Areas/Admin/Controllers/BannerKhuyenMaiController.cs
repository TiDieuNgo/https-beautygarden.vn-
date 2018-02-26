using System;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class BannerKhuyenMaiController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBannerKhuyenMaiRepository _bannerKhuyenMaiRepository;

        public BannerKhuyenMaiController(IUnitOfWork unitOfWork, IBannerKhuyenMaiRepository bannerKhuyenMaiRepository)
        {
            _unitOfWork = unitOfWork;
            _bannerKhuyenMaiRepository = bannerKhuyenMaiRepository;
        }

        public ActionResult Index()
        {
            var dl = _bannerKhuyenMaiRepository.GetAll();
            return View(dl);
        }

        public ActionResult Edit(int id)
        {
            var dlcu = _bannerKhuyenMaiRepository.GetById(id);
            return View("Edit", dlcu);
        }
        public ActionResult Add()
        {
            return View("Create");
        }
        //them moi tu danh sach tag ngoai menu admin
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(ThemBannerKhuyenMaiModel model)
        {
            if (ModelState.IsValid)
            {
                BannerKhuyenMai menu = new BannerKhuyenMai()
                {
                    HinhAnh = (model.HinhAnh).Replace("/files/", ""),
                    MoTa = model.MoTa,
                    NgayTao = DateTime.Now,
                    Showhide = true,
                    Ten = model.Ten
                };
                _bannerKhuyenMaiRepository.Add(menu);
                _unitOfWork.Commit();
            }
            else
            {
                return View("Create", model);
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(BannerKhuyenMai model)
        {
            BannerKhuyenMai dlcu = _bannerKhuyenMaiRepository.GetById(model.Id);
            dlcu.Id = model.Id;
            dlcu.Ten = model.Ten;
            dlcu.NgayTao = DateTime.Now;
            dlcu.Showhide = true;
            dlcu.MoTa = model.MoTa;
            dlcu.HinhAnh = (model.HinhAnh).Replace("/files/", "");
            _bannerKhuyenMaiRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");

        }
        public ActionResult delete(int id)
        {
            var idcu = _bannerKhuyenMaiRepository.GetById(id);
            _bannerKhuyenMaiRepository.Delete(idcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
