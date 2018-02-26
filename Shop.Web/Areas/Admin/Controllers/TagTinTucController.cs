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
    public class TagTinTucController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITagTinTucRepository _tagTinTucRepository;

        public TagTinTucController(IUnitOfWork unitOfWork, ITagTinTucRepository tagTinTucRepository)
        {
            _unitOfWork = unitOfWork;
            _tagTinTucRepository = tagTinTucRepository;
        }
        private const int pageSize = 50;

        public ActionResult Index(int page = 1)
        {
            int pageNumber = page;
            var dl = _tagTinTucRepository.GetAll().OrderByDescending(o=>o.NgayTao).ToPagedList(pageNumber, pageSize);
            return View(dl);
        }

        public ActionResult Edit(int id)
        {
            var dlcu = _tagTinTucRepository.GetById(id);
            return View("Edit", dlcu);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(TagTinTuc model)
        {
            TagTinTuc dlcu = _tagTinTucRepository.GetById(model.Id);
            dlcu.Seodcription = model.Seodcription;
            dlcu.Id = model.Id;
            dlcu.IdMenu = model.IdMenu;
            dlcu.NgayTao = DateTime.Now;
            dlcu.Seotitle = model.Seotitle;
            _tagTinTucRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var idxoa = _tagTinTucRepository.GetById(id);
            _tagTinTucRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        //tim kiem tag
        public ActionResult TimKiem(string keyword, int? page)
        {
            int pageNumber = (page ?? 1);
            keyword = RejectMarks(keyword);
            IList<TagTinTuc> danhSachTags = _tagTinTucRepository.TimKiem(keyword.Trim().ToLower());
            TagTinTucModelTimKiem timkiem = new TagTinTucModelTimKiem()
                                              {
                                                  DanhSachTags = danhSachTags.ToPagedList(pageNumber, pageSize),
                                                  soluongtimduoc = danhSachTags.Count()
                                              };
            ViewData["key"] = keyword;
            return View(timkiem);
        }
    }
}
