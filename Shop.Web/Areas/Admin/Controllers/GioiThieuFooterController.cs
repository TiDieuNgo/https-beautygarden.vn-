using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
   [ShopAuthorize]
    public class GioiThieuFooterController : BaseController
    {
        private readonly ICauHinhRepository _cauHinhRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GioiThieuFooterController(ICauHinhRepository cauHinhRepository, IUnitOfWork unitOfWork)
        {
            _cauHinhRepository = cauHinhRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {

            IList<CauHinh> tags = _cauHinhRepository.GetAll().ToList();
            return View("Index", tags);

        }
        public ActionResult Add()
        {
            return View("Create");
        }
        //them moi
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(CauHinhModel objectMenu)
        {

            if (ModelState.IsValid)
            {
                CauHinh model = new CauHinh()
                {
                   
                    NoiDung = objectMenu.NoiDung,
                    TieuDe = objectMenu.TieuDe
                };
                _cauHinhRepository.Add(model);
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
            var idcu = _cauHinhRepository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(CauHinh model)
        {
            CauHinh menu = _cauHinhRepository.GetById(model.Id);
            menu.Id = model.Id;
            menu.TieuDe = model.TieuDe;
            menu.NoiDung = model.NoiDung;
            _cauHinhRepository.Update(menu);
            _unitOfWork.Commit();
           
            return RedirectToAction("Index");
        }
        public ActionResult delete(int id)
        {
            var idxoa = _cauHinhRepository.GetById(id);
            _cauHinhRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
      
       
    }
}
