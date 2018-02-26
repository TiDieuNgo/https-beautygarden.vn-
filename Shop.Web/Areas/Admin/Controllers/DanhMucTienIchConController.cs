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
    public class DanhMucTienIchConController : BaseController
    {

        private readonly IDanhMucTienIchRepository _danhMucTienIchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DanhMucTienIchConController(IDanhMucTienIchRepository danhMucTienIchRepository, IUnitOfWork unitOfWork)
        {
            _danhMucTienIchRepository = danhMucTienIchRepository;
            _unitOfWork = unitOfWork;
        }

        private const int pageSize = 20;
        
     
        public ActionResult Index(int? page, int id)
        {

            int pageNumber = (page ?? 1);
            IList<DanhMucTienIch> dm =
                _danhMucTienIchRepository.GetMany(o => o.Idcontrol == id).OrderBy(o=>o.SapXep).ToList();
            var data = dm.ToPagedList(pageNumber, pageSize);
            //lay ten danh muc sau khi click vao submenu de hien thi breadcrumb
            ViewData["DuongDanDanhMuc"] = _danhMucTienIchRepository.GetTenDanhMucById(id);
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

            DanhMucTienIch menu = new DanhMucTienIch()
                            {
                                Idcontrol = IdMenu, //danh muc con cap 1
                                ok = false,
                                NgayTao = DateTime.Now,
                                IconDanhMuc = "",
                                SapXep = IPosition,
                                TenDanhMuc = NameProduct
                              
                            };
            _danhMucTienIchRepository.Add(menu);
            _unitOfWork.Commit();

            return Json(new
            {

                message = "Thêm mới thành công!"
            }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost, ValidateInput(false)]
        public JsonResult Delete(int id)
        {
            var idxoa = _danhMucTienIchRepository.GetById(id);
            _danhMucTienIchRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return Json(new
            {

                message = "Xóa thành công!"
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {

            var idcu = _danhMucTienIchRepository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult SaveEditSubmenu1json(DanhMucTienIch model, int IdMenu, string NameProduct, int IPosition)
        {
            //lay cha cua id san pham dang sua
            int idcha = _danhMucTienIchRepository.GetIdDanhMucCha(IdMenu);
            DanhMucTienIch menu = _danhMucTienIchRepository.GetById(IdMenu);
            menu.id_ = IdMenu;
            menu.TenDanhMuc = NameProduct;
            menu.SapXep = IPosition;

            _danhMucTienIchRepository.Update(menu);
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
                    context.Database.ExecuteSqlCommand("update DanhMucTienIchs set ok='True' where id_ ={0}", idsanpham);

                }
            }
            else
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update DanhMucTienIchs set ok='False' where id_ ={0}", idsanpham);

                }
            }

            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }
    }
}