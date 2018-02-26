using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class SubMenu2Controller : BaseController
    {
        private int iddanhmuc = 0;
        private readonly IMenuRepository _menuRepository;
        private readonly IMenuProAddRepository _menuProAddRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;

        public SubMenu2Controller(IMenuRepository menuRepository, IMenuProAddRepository menuProAddRepository,
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
                _menuRepository.GetMany(o => o.idControl == id && o.Style == "danh-muc-san-pham").OrderByDescending(
                    o => o.sDate).ToList();
            var data = dm.ToPagedList(pageNumber, pageSize);
            //lay ten danh muc sau khi click vao submenu de hien thi breadcrumb
            ViewData["DuongDanCha"] = _menuRepository.GetTenDanhMucChaById(id);//duong dan danh muc cha
            ViewData["DuongDanDanhMuc"] = _menuRepository.GetTenDanhMucById(id);
            ViewData["LayIdDMcha"] = _menuRepository.GetIdDanhMucCha(id);
            ViewData["id"] = id;
            return View("Index", data);
        }

        public ActionResult Add()
        {
         
            return View("Create");
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult Addnew(int IdMenu, string NameProduct, int IPosition)
        {

            Menu menu = new Menu()
                            {
                                //admin/submenu1/index/1240
                                idControl = IdMenu, //danh muc con cap 1
                                VIP = false,
                                Link = ConvertFont(NameProduct),
                                Style = "danh-muc-san-pham",
                                ui = "vi",
                                IdNhaCungCap = 12,
                                Visitor = 0,
                                ok = true,
                                sDate = DateTime.Now,
                                sDateOk = DateTime.Now,
                                idUser = 18,
                                idUserOk = 18,
                                CodeProduct = RejectMarks(NameProduct),
                                NameProduct = NameProduct,
                                SapxepDanhMuc = IPosition,
                                ShowMenuHome = true,
                                ShowMenuTop = true
                            };
            _menuRepository.Add(menu);
            _unitOfWork.Commit();
            return Json(new
            {

                message = "Thêm mới thành công!"
            }, JsonRequestBehavior.AllowGet);
        }
            [HttpPost, ValidateInput(false)]
        public JsonResult Delete(int id)
        {
            var idxoa = _menuRepository.GetById(id);
            _menuRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return Json(new
            {

                message = "Xóa thành công!"
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {
            var idcu = _menuRepository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEditSubmenu2(Menu model)
        {
            Menu menu = _menuRepository.GetById(model.id_);
            menu.id_ = model.id_;
            menu.NameProduct = model.NameProduct;

            menu.SapxepDanhMuc = model.SapxepDanhMuc;

            _menuRepository.Update(menu);
            _unitOfWork.Commit();
            return RedirectToAction("ViewDanhMuc", "DanhMucSanPham");
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult SaveEditSubmenu2json(Menu model, int IdMenu, string NameProduct, int IPosition,string SEODescription,string SEOtitle)
        {
            //lay cha cua id san pham dang sua
            int idcha = _menuRepository.GetIdDanhMucCha(IdMenu);//cha 1
            int idcharoot = _menuRepository.GetIdDanhMucCha(idcha);
            Menu menu = _menuRepository.GetById(IdMenu);
            menu.id_ = IdMenu;
            menu.NameProduct = NameProduct;
            menu.SapxepDanhMuc = IPosition;
            menu.SEODescription = SEODescription;
            menu.SEOtitle = SEOtitle;
            _menuRepository.Update(menu);
            _unitOfWork.Commit();
            return Json(new
            {
                idcha,
                message = "Cập nhật thành công!"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateOk(int idsanpham, bool Checked)
        {
            if (Checked)
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set ok='True' where id_ ={0}", idsanpham);

                }
            }
            else
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Menu set ok='False' where id_ ={0}", idsanpham);

                }
            }

            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }
    }
}