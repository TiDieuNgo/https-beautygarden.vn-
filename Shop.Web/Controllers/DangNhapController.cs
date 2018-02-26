using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.Common;
using Shop.Web.Model;

namespace Shop.Web.Controllers
{
    public class DangNhapController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccMember298Repository _accMember298Repository;

        public DangNhapController(IUnitOfWork unitOfWork, IAccMember298Repository accMember298Repository)
        {
            _unitOfWork = unitOfWork;
            _accMember298Repository = accMember298Repository;
        }
        public ActionResult Index()
        {
            return View("dangnhap");
        }
        public ActionResult Submit()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Submit(DanhNhapModel model)
        {
            AccMember298 khachHang = _accMember298Repository.Get(o => o.Email.Equals(model.Email));
            if (khachHang != null)
            {
                string matKhau = Md5Encrypt.Md5EncryptPassword(model.Password);
                if (khachHang.Password.Equals(matKhau))
                {
                    //dang nhap thanh cong
                    SetCookieLogin(this.Request.RequestContext, model.Email);
                    ViewData["Message"] = "Đăng nhập thành công!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["Message"] = "Mật Khẩu Sai";
                    return View("dangnhap", model);
                }
            }
            else
            {
                ViewData["Message"] = "Email không tồn tại";
                return View("dangnhap", model);
            }

        }
        public ActionResult LogOut()
        {
            SetCookieLogin(this.Request.RequestContext, null);
            return Redirect("/");
        }
    }
}
