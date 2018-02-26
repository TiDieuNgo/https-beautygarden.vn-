using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
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
    public class YKienKhachHangController : BaseController
    {
        private readonly IYKienKhachHangRepository _kienKhachHangRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;
        private readonly HttpContextBase _httpContext;
        public YKienKhachHangController(IYKienKhachHangRepository kienKhachHangRepository, IUnitOfWork unitOfWork, HttpContextBase httpContext)
        {
            _kienKhachHangRepository = kienKhachHangRepository;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<YKienKhachHang> tags = _kienKhachHangRepository.GetAll().OrderByDescending(o => o.NgayTao).ToList();
            var data = tags.ToPagedList(pageNumber, pageSize);
            return View("Index", data);

        }
        public ActionResult Add()
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Ykienkhachhang_Create)))
                return View("_NoAuthor");
            return View("Create");
        }
        //them moi
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(YKienKhachHangModel objectMenu)
        {

            if (ModelState.IsValid)
            {
                YKienKhachHang model = new YKienKhachHang()
                {


                    HinhDaiDien = (objectMenu.HinhDaiDien).Replace("/files/", ""),
                    NgayTao = DateTime.Now,
                    LinkFaceBook = objectMenu.LinkFaceBook,
                    MoTa = objectMenu.MoTa,
                    NoiDung = objectMenu.NoiDung,
                    TenKhachHang = objectMenu.TenKhachHang,

                };
                _kienKhachHangRepository.Add(model);
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
            if (!CheckRole(_httpContext, int.Parse(Roles.Ykienkhachhang_Edit)))
                return View("_NoAuthor");
            var idcu = _kienKhachHangRepository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(YKienKhachHang model)
        {
            YKienKhachHang menu = _kienKhachHangRepository.GetById(model.id_);
            menu.id_ = model.id_;
            menu.TenKhachHang = model.TenKhachHang;
            menu.HinhDaiDien = model.HinhDaiDien;
            menu.MoTa = model.MoTa;
            menu.LinkFaceBook = model.LinkFaceBook;
            menu.NoiDung = model.NoiDung;
            menu.NgayTao = DateTime.Now;
            _kienKhachHangRepository.Update(menu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult delete(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Ykienkhachhang_Delete)))
                return View("_NoAuthor");
            var idxoa = _kienKhachHangRepository.GetById(id);
            _kienKhachHangRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
