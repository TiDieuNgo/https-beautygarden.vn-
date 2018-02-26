using System;
using System.Linq;
using System.Web.Mvc;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
     [ShopAuthorize]
    public class DanhMucTienIchController : BaseController
    {
        private readonly IDanhMucTienIchRepository _danhMucTienIchRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;
      
        public DanhMucTienIchController(IDanhMucTienIchRepository danhMucTienIchRepository, IUnitOfWork unitOfWork)
        {
            _danhMucTienIchRepository = danhMucTienIchRepository;
            _unitOfWork = unitOfWork;
        }
        
        public ActionResult Index()
        {

            var dl = _danhMucTienIchRepository.GetMany(o=>o.Idcontrol==0).OrderBy(o=>o.SapXep).ToList();
            return View("Index",dl);
        }
        
        //them moi danh muc
        public ActionResult Add()
        {
            return View("Create");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Addnew(DanhMucTienIchModel model)
        {
            if (ModelState.IsValid)
            {
                DanhMucTienIch danhMuc = new DanhMucTienIch()
                                             {
                                                 IconDanhMuc = model.IconDanhMuc.Replace("/files/", ""),
                                                 NgayTao = DateTime.Now,
                                                 Idcontrol = 0,
                                                 SapXep = model.SapXep,
                                                 TenDanhMuc = model.TenDanhMuc,
                                                 ok = false //luc dau an no di
                                             };
                _danhMucTienIchRepository.Add(danhMuc);
                _unitOfWork.Commit();
            }
            else
            {
                return View("Create", model);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var idxoa = _danhMucTienIchRepository.GetById(id);
            _danhMucTienIchRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var idcu = _danhMucTienIchRepository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(DanhMucTienIch model)
        {
            DanhMucTienIch dlcu = _danhMucTienIchRepository.GetById(model.id_);
            dlcu.id_ = model.id_;
            dlcu.TenDanhMuc = model.TenDanhMuc;
            dlcu.IconDanhMuc = (model.IconDanhMuc).Replace("/files/", "");
            dlcu.SapXep = model.SapXep;
            dlcu.NgayTao = DateTime.Now;
            _danhMucTienIchRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
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