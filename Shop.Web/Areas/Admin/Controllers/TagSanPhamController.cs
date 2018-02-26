using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class TagSanPhamController : BaseController
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDanhSachTagRepository _danhSachTagRepository;
        private const int pageSize = 50;
        public TagSanPhamController(IMenuRepository menuRepository, IUnitOfWork unitOfWork, IDanhSachTagRepository danhSachTagRepository)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
            _danhSachTagRepository = danhSachTagRepository;
        }

        public ActionResult Index(int page = 1)
        {
            int pageNumber = page;
            var dl = _danhSachTagRepository.GetAll().OrderByDescending(o=>o.NgayTao).ToPagedList(pageNumber, pageSize);
            return View(dl);
        }

        public ActionResult Edit(int id)
        {
            var dlcu = _danhSachTagRepository.GetById(id);
            return View("Edit", dlcu);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(DanhSachTag model)
        {
            DanhSachTag dlcu = _danhSachTagRepository.GetById(model.Id);
            dlcu.SEODescription = model.SEODescription;
            dlcu.Id = model.Id;
            dlcu.IdMenu = model.IdMenu;
            dlcu.NgayTao = DateTime.Now;
            dlcu.SEOtitle = model.SEOtitle;
            dlcu.NguoiTao = 18;
            _danhSachTagRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var idxoa = _danhSachTagRepository.GetById(id);
            _danhSachTagRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        //tim kiem tag
        public ActionResult TimKiem(string keyword, int? page)
        {
            int pageNumber = (page ?? 1);
            keyword = RejectMarks(keyword);
            IList<DanhSachTag> danhSachTags = _danhSachTagRepository.TimKiem(keyword.Trim().ToLower());
            DanhSachTagModelTimKiem timkiem = new DanhSachTagModelTimKiem()
                                  {
                                      DanhSachTags = danhSachTags.ToPagedList(pageNumber, pageSize),
                                      soluongtimduoc = danhSachTags.Count()
                                  };
            ViewData["key"] = keyword;
            return View(timkiem);
        }
    }
}
