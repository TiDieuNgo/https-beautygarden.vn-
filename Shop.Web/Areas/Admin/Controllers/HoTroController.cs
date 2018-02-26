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
    public class HoTroController : BaseController
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;
        public HoTroController(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<Menu> tags = _menuRepository.GetMany(o => o.Style == "ho-tro" && o.idControl != 0).OrderBy(o => o.sPosition).ToList();
            var data = tags.ToPagedList(pageNumber, pageSize);
            return View("Index", data);

        }
        public ActionResult Add()
        {
            return View("Create");
        }
        //them moi
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(VeChungToiModel objectMenu)
        {

            if (ModelState.IsValid)
            {
                Menu model = new Menu()
                {
                    CodeProduct = RejectMarks(objectMenu.NameProduct),

                  
                    IdNhaCungCap = 12,
                    idControl = 17,
                    ok = true,
                    Link = ConvertFont(objectMenu.NameProduct),
                    LevelMenu = 1,
                    LinkHttp = "https://beautygarden.vn/" + ConvertFont(objectMenu.NameProduct) + ".html",
                    Style = "ho-tro",
                    ui = "vi",
                    sPosition = objectMenu.sPosition,
                    sDate = DateTime.Now,
                    sDateOk = DateTime.Now,
                    idUser = 18,
                    idUserOk = 18,
                    NameProduct = objectMenu.NameProduct,
                    Content = objectMenu.Content

                };
                _menuRepository.Add(model);
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
            var idcu = _menuRepository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(Menu model)
        {
            Menu menu = _menuRepository.GetById(model.id_);
            menu.id_ = model.id_;
            menu.NameProduct = model.NameProduct;
            menu.Content = model.Content;
            menu.sPosition = model.sPosition;
            _menuRepository.Update(menu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult delete(int id)
        {
            var idxoa = _menuRepository.GetById(id);
            _menuRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
      
       
    }
}
