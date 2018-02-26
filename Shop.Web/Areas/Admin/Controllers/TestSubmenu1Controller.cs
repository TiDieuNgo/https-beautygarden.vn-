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

namespace Shop.Admin.Controllers
{
    //[ShopAuthorize]
    public class TestSubmenu1Controller : BaseController
    {
        public int iddanhmuc = 0;
        private readonly IMenuRepository _menuRepository;
        private readonly IMenuProAddRepository _menuProAddRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;

        public TestSubmenu1Controller(IMenuRepository menuRepository, IMenuProAddRepository menuProAddRepository,
                                  IUnitOfWork unitOfWork)
        {
            _menuRepository = menuRepository;
            _menuProAddRepository = menuProAddRepository;
            _unitOfWork = unitOfWork;
        }

        //submenu 1
        public ActionResult Index(int? page, int id)
        {

            int pageNumber = (page ?? 1);
            IList<Menu> dm =
                _menuRepository.GetMany(o => o.idControl == id && o.Style == "danh-muc-san-pham").OrderByDescending(o => o.sDate).ToList();
            var data = dm.ToPagedList(pageNumber, pageSize);
            DanhMucConModel model=new DanhMucConModel()
                                      {
                                          DanhMucs =   _menuRepository.GetMany(o => o.idControl == id && o.Style == "danh-muc-san-pham").OrderByDescending(o => o.sDate).ToPagedList(pageNumber,pageSize),
                                          Menu = _menuRepository.GetById(id)
                                      };
            //lay ten danh muc sau khi click vao submenu de hien thi breadcrumb
            ViewData["DuongDanDanhMuc"] = _menuRepository.GetTenDanhMucById(id);

            return View("Index", model);
        }

        public ActionResult Add(int id)
        {
            var idcu = _menuRepository.GetById(id);
            return View("Create",idcu);
           
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Addnew(Menu model)
        {
            Menu menuid = _menuRepository.GetById(model.id_);
            Menu menu = new Menu()
                            {
                               
                                //admin/submenu1/index/1240
                                idControl = menuid.id_, //danh muc con cap 1
                                VIP = false,
                                Menu2 = "",
                                MenuAdwords = "",
                                Link = ConvertFont(model.NameProduct),
                                LevelMenu = 2,
                                LinkHttp = "",
                                LinkHttp1 = "",
                                Note = "",
                                Img = "",
                                Style = "danh-muc-san-pham",
                                ui = "vi",
                                ContentLabel = "",
                                Content = "",
                                Option = true,
                                ContentLabel1 = "",
                                Content1 = "",
                                Option1 = true,
                                ContentLabel2 = "",
                                Content2 = "",
                                Option2 = true,
                                ContentLabel3 = "",
                                Content3 = "",
                                ContentLabel4 = "",
                                Content4 = "",
                                Option4 = false,
                                Option5 = false,
                                Option6 = false,
                                Option7 = false,
                                Option8 = true,
                                Option9 = false,
                                sPosition = model.sPosition,
                                IdNhaCungCap = 12,
                                Visitor = 0,
                                ok = true,
                                sDate = DateTime.Now,
                                sDateOk = DateTime.Now,
                                idUser = 18,
                                idUserOk = 18,
                                SEOKeyWord = "",
                                SEODescription = "",
                                NumberHaveGift = 0,
                                idMPADSys = 0,
                                PriceOffPro = 0,
                                CodeProduct = RejectMarks(model.NameProduct),
                                NameProduct = model.NameProduct,
                                ShowMenuHome = model.ShowMenuHome,
                                ShowMenuTop = model.ShowMenuTop,
                                SapxepDanhMuc = model.SapxepDanhMuc,
                                SapXepSanPham = model.SapXepSanPham
                            };
            _menuRepository.Add(menu);
            _unitOfWork.Commit();
            return RedirectToAction("ViewDanhMuc", "DanhMucSanPham");
        }
        public ActionResult Delete(int id)
        {
            var idxoa = _menuRepository.GetById(id);
            _menuRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("ViewDanhMuc", "DanhMucSanPham");
        }
        public ActionResult Edit(int id)
        {
            var idcu = _menuRepository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEditSubmenu1(Menu model)
        {
            Menu menu = _menuRepository.GetById(model.id_);
            menu.id_ = model.id_;
            menu.NameProduct = model.NameProduct;
            menu.ShowMenuTop = model.ShowMenuTop;
            menu.ShowMenuHome = model.ShowMenuHome;
            menu.SapxepDanhMuc = model.SapxepDanhMuc;
            menu.SapXepSanPham = model.SapXepSanPham;
            _menuRepository.Update(menu);
            _unitOfWork.Commit();
            return RedirectToAction("ViewDanhMuc", "DanhMucSanPham");
        }
    }
}