using System;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;

namespace Shop.Web.Controllers
{
    [WhitespaceFilter]
    public class HomeController : BaseController
    {
        private readonly IAccMember298Repository _accMember298Repository;
        private readonly IMenuRepository _menuRepository;
        private readonly ITuKhoaTimKiemRepository _tuKhoaTimKiemRepository;
        private readonly IUserRatingRepository _userRatingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IAccMember298Repository accMember298Repository, IMenuRepository menuRepository, IUserRatingRepository userRatingRepository, IUnitOfWork unitOfWork)
        {
            _accMember298Repository = accMember298Repository;
            _menuRepository = menuRepository;
            _userRatingRepository = userRatingRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            Menu bannerQCtrangchu =
                _menuRepository.GetMany(o => o.idControl == 25 && o.Style == "banner-website-danhsach" && o.ok).FirstOrDefault();
            return View("Index", bannerQCtrangchu);
        }
        public ActionResult GetUser()
        {
            string email = GetCookieLogin(this.HttpContext);
            AccMember298 khachHang = null;
            if (!string.IsNullOrEmpty(email))
            {
                khachHang = _accMember298Repository.Get(o => o.Email.Equals(email));
                return View("User", khachHang);
            }
            return View("User", khachHang);
        }
        [HttpPost]
        public JsonResult GetTopSearch()
        {
            try
            {
                var data = Shop.Web.Model.YKienKhachHangModel.GetListTop10();
                return Json(new { data = data, status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, status = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SaveRating(float diem, int IdSanPham)
        {
            if (Request.Cookies["A" + IdSanPham.ToString()] == null)
            {
                Response.Cookies["A" + IdSanPham.ToString()].Value = IdSanPham.ToString();
                Response.Cookies["A" + IdSanPham.ToString()].Expires = DateTime.Now.AddDays(1);

                diem = diem <= 4 ? 4 : diem;
                UserRating _danhgia = new UserRating()
                {
                    IdSanPham = IdSanPham,
                    Rating = diem,
                    UserAcount = User.Identity.Name == null ? "" : User.Identity.Name
                };
                _userRatingRepository.Add(_danhgia);
                _unitOfWork.Commit();
            }
            else if (Request.Cookies["A" + IdSanPham.ToString()].Value != IdSanPham.ToString())
            {
                Response.Cookies["A" + IdSanPham.ToString()].Value = IdSanPham.ToString();
                Response.Cookies["A" + IdSanPham.ToString()].Expires = DateTime.Now.AddDays(1);

                diem = diem <= 4 ? 4 : diem;
                UserRating _danhgia = new UserRating()
                {
                    IdSanPham = IdSanPham,
                    Rating = diem,
                    UserAcount = User.Identity.Name == null ? "" : User.Identity.Name
                };
                _userRatingRepository.Add(_danhgia);
                _unitOfWork.Commit();
            }

            return Json(new { message = "ok" }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Robots()
        {
            Response.ContentType = "text/plain";
            return View();
        }
    }
}
