using System;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class TinKhuyenMaiController : Controller
    {
        private readonly IDetailMenuRepository _detailMenuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;
        public TinKhuyenMaiController(IDetailMenuRepository detailMenuRepository, IUnitOfWork unitOfWork)
        {
            _detailMenuRepository = detailMenuRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult add()
        {
            return View("Create");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(DetailMenu model)
        {
            model.Name = model.Name;
            model.Img = model.Img;
            model.Link = model.Link;
            model.id_Menu = 1399;
            model.Ma_Hang = "";
            model.NameAdwords = "";
            model.ok = true;
            model.Number = 0;
            model.ShowMenu = true;
            model.ShowKhuyenMai = false;
            model.ShowIconHot = false;
            model.ShowIconNew = false;
            model.TinhTrangSP = false;
            model.idUser = 15;
            model.idUserOk = 15;
            model.sDate = DateTime.Now;
            model.sDateOk = DateTime.Now;

            _detailMenuRepository.Add(model);
            _unitOfWork.Commit();
            return RedirectToAction("TinKhuyenMai", "TinTuc");

        }

        public ActionResult Edit(int id)
        {

            var dlcu = _detailMenuRepository.GetById(id);
            return View("Edit", dlcu);

        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(DetailMenu model)
        {
            DetailMenu dlcu = _detailMenuRepository.GetById(model.id_);
            dlcu.id_ = model.id_;
            dlcu.Name = model.Name;
            dlcu.Img = model.Img;
            dlcu.Link = model.Link;
            dlcu.id_ = model.id_;
            _detailMenuRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("TinKhuyenMai", "TinTuc"); 
        }
        public ActionResult delete(int id)
        {
            var idxoa = _detailMenuRepository.GetById(id);
            _detailMenuRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("TinKhuyenMai", "TinTuc");
        }
    }
}
