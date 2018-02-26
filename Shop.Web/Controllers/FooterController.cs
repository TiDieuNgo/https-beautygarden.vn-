using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Models;


namespace Shop.Web.Controllers
{
    public class FooterController : Controller
    {
        private readonly ICauHinhRepository _cauHinhRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITuKhoaTimKiemRepository _tuKhoaTimKiemRepository;
        private readonly ISEOFooterRepository _seoFooterRepository;
        public FooterController(IMenuRepository menuRepository, IUnitOfWork unitOfWork, IEmailRepository emailRepository, ITuKhoaTimKiemRepository tuKhoaTimKiemRepository, ICauHinhRepository cauHinhRepository, ISEOFooterRepository seoFooterRepository)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
            _emailRepository = emailRepository;
            _tuKhoaTimKiemRepository = tuKhoaTimKiemRepository;
            _cauHinhRepository = cauHinhRepository;
            _seoFooterRepository = seoFooterRepository;
        }

        public ActionResult Index()
        {
            FooterModel model = new FooterModel()
            {
                VeChungTois = _menuRepository.GetMany(o => o.Style == "ve-chung-toi" && o.ok && o.idControl != 0).OrderByDescending(o => o.sDate).Take(4).ToList(),
                HoTros = _menuRepository.GetMany(o => o.Style == "ho-tro" && o.ok && o.idControl != 0).OrderByDescending(o => o.sDate).Take(4).ToList(),
                TuKhoaTimKiems = _tuKhoaTimKiemRepository.DanhSachTuKhoa(),
                CauHinh = _cauHinhRepository.GetAll().FirstOrDefault()
            };
            return View("FooterPartial", model);
        }
        public ActionResult SEOFooter()
        {
            IList<SEOFooter> seoFooters = _seoFooterRepository.GetAll().OrderByDescending(o => o.NgayTao).ToList();
            return View("SEOFooter", seoFooters);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult SaveEmailKM(string Emails)
        {
            Email mail = new Email()
            {
                Emails = Emails,
                SDate = DateTime.Now
            };
            _emailRepository.Add(mail);
            _unitOfWork.Commit();
            return Json(new
            {
                message = "Bạn đã đăng ký thành công!"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
