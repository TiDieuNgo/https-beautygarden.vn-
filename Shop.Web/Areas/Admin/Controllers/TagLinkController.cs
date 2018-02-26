using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;

namespace Shop.Admin.Controllers
{
    // [ShopAuthorize]
    public class TagLinkController : Controller
    {
        private readonly ITagsLinkRepository _tagsLinkRepository;
        private readonly IMenuRepository _menuRepository;

        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;
        public TagLinkController(ITagsLinkRepository tagsLinkRepository, IUnitOfWork unitOfWork, IMenuOptionRepository menuOptionRepository, IMenuRepository menuRepository)
        {
            _tagsLinkRepository = tagsLinkRepository;
            _unitOfWork = unitOfWork;
            _menuRepository = menuRepository;
        }

        public ActionResult Index()
        {
            return View("taglink");
        }
        //danh sach tag hien thi ngoai menu admin
        public ActionResult DanhSachTag(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<TagMapping> tags = _tagsLinkRepository.GetTenSanPhamInMenu().ToList();
            var data = tags.ToPagedList(pageNumber, pageSize);
            return View("Index", data);
        }
        public ActionResult Edit(int id)
        {

            var dlcu = _tagsLinkRepository.GetById(id);
            return View("Edit", dlcu);

        }
        public ActionResult add()
        {

            var sanphamlist = new List<SelectListItem>() { new SelectListItem() { Text = "---chọn sản phẩm---", Value = "0" } };
            IList<Menu> sanPhams = _menuRepository.GetAll().OrderByDescending(o => o.sDate).ToList();
            sanphamlist.AddRange(from o in sanPhams where o.idControl == 11 && o.HasValue && o.HasOnHand && o.ok select new SelectListItem() { Text = o.NameProduct, Value = o.id_.ToString() });

            return View("Create", new TaglinkModel()
            {

                DanhMucs = sanphamlist
            });

        }
        //them moi tu danh sach tag ngoai menu admin
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(TagsLink model)
        {
            TagsLink tag = new TagsLink()
            {
                Link = model.Link,
                ok = true,
                sDateOk = DateTime.Now,
                SDate = DateTime.Now,
                TenTags = model.TenTags,
                IdMenu = model.IdMenu,
                idControl = 1,
                idUser = 18,
                idUserOk = 18,
            };
            _tagsLinkRepository.Add(tag);
            _unitOfWork.Commit();
            return RedirectToAction("DanhSachTag");
        }

        public ActionResult delete(int id)
        {
            var idcu = _tagsLinkRepository.GetById(id);
            _tagsLinkRepository.Delete(idcu);
            _unitOfWork.Commit();
            return RedirectToAction("DanhSachTag");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(TagMapping model)
        {
            return RedirectToAction("Index");
        }

    }
}
