using System;
using System.Linq;
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
    public class HeThongCuaHangController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeThongCuaHangRepository _heThongCuaHangRepository;

        public HeThongCuaHangController(IUnitOfWork unitOfWork, IHeThongCuaHangRepository heThongCuaHangRepository)
        {
            _unitOfWork = unitOfWork;
            _heThongCuaHangRepository = heThongCuaHangRepository;
        }

        public ActionResult Index()
        {
            var dl =
                _heThongCuaHangRepository.GetAll().OrderBy(o => o.SapXep);
            return View(dl);
        }

        public ActionResult Edit(int id)
        {
            var dlcu = _heThongCuaHangRepository.GetById(id);
            return View("Edit", dlcu);
        }
        public ActionResult Add()
        {
            return View("Create");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(HeThongCuaHangModel model)
        {
            if (ModelState.IsValid)
            {
                HeThongCuaHang menu = new HeThongCuaHang()
                {
                    HinhAnh = (model.HinhAnh).Replace("/files/", ""),
                    DiaChi = model.DiaChi,
                    NgayTao = DateTime.Now,
                    SapXep = model.SapXep,
                    TenShop = model.TenShop
                };
                _heThongCuaHangRepository.Add(menu);
                _unitOfWork.Commit();
            }
            else
            {
                return View("Create", model);
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(HeThongCuaHang model)
        {
            HeThongCuaHang dlcu = _heThongCuaHangRepository.GetById(model.Id);
            dlcu.Id = model.Id;
            dlcu.DiaChi = model.DiaChi;
            dlcu.SapXep = model.SapXep;
            dlcu.NgayTao = DateTime.Now;
            dlcu.HinhAnh = (model.HinhAnh).Replace("/files/", "");
            dlcu.TenShop = model.TenShop;
            _heThongCuaHangRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult delete(int id)
        {
            var idcu = _heThongCuaHangRepository.GetById(id);
            _heThongCuaHangRepository.Delete(idcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
