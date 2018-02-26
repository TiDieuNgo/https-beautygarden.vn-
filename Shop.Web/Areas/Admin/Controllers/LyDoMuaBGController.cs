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
    public class LyDoMuaBGController : BaseController
    {
        private readonly IThietLapRepository _thietLapRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LyDoMuaBGController(IThietLapRepository thietLapRepository, IUnitOfWork unitOfWork)
        {
            _thietLapRepository = thietLapRepository;
            _unitOfWork = unitOfWork;
        }

      
      
        public ActionResult Index()
        {

            IList<ThietLap> tags = _thietLapRepository.GetAll().ToList();
          
            return View("Index", tags);

        }
        public ActionResult Add()
        {
            return View("Create");
        }
        //them moi
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(LyDoMuaBGModel objectMenu)
        {

            if (ModelState.IsValid)
            {
                ThietLap model = new ThietLap()
                {
                   
                    NoiDung = objectMenu.NoiDung,
                    TieuDe = objectMenu.TieuDe
                };
                _thietLapRepository.Add(model);
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
            var idcu = _thietLapRepository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(ThietLap model)
        {
            ThietLap menu = _thietLapRepository.GetById(model.ThietLapId);
            menu.ThietLapId = model.ThietLapId;
            menu.TieuDe = model.TieuDe;
            menu.NoiDung = model.NoiDung;
            _thietLapRepository.Update(menu);
            _unitOfWork.Commit();
           
            return RedirectToAction("Index");
        }
        public ActionResult delete(int id)
        {
            var idxoa = _thietLapRepository.GetById(id);
            _thietLapRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
      
       
    }
}
