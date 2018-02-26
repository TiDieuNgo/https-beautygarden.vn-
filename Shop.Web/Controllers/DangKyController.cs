using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.Common;
using Shop.Web.Core.Helper;
using Shop.Web.Models;


namespace Shop.Web.Controllers
{
    public class DangKyController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccMember298Repository _accMember298Repository;

        public DangKyController(IAccMember298Repository accMember298Repository, IUnitOfWork unitOfWork)
        {
            _accMember298Repository = accMember298Repository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            CookieHelper cookieHelper = new CookieHelper("User_Infor");

            KhachHangModel model = new KhachHangModel();
            if (cookieHelper.GetCookie() != null)
            {

                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["User_Infor"]);
                if (!string.IsNullOrEmpty(json))
                {
                    KhachHangModel form = JsonConvert.DeserializeObject<KhachHangModel>(json);
                    model.Phone = form.Phone;
                    model.Email = form.Email;
                }
            }

            return View("dangky", model);
        }
        public ActionResult Submit()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Submit(KhachHangModel luuVao)
        {
            if (ModelState.IsValid)
            {
                AccMember298 check = _accMember298Repository.Get(o => o.Email.Equals(luuVao.Email.Trim()));//kt email da ton tai chua
                if (check == null)//neu rong thi no them vao csdl binh thuong 
                {
                    AccMember298 khachHang = new AccMember298()
                    {
                        Fullname = "",
                        Username = "user",
                        Email = luuVao.Email,
                        Phone = luuVao.Phone,
                        Password = Md5Encrypt.Md5EncryptPassword(luuVao.Password),
                        sDate = DateTime.Now,
                        sDateOk = DateTime.Now,
                        idUser = 15,
                        idUserOk = 15,
                        Sex = "Nữ",
                        Birdthday = DateTime.Now,
                        Address = "",
                        States = "",
                        Level = 0,
                        Show = true,
                        DiaChiGiaoHang = "",
                        TenCongTy = "",
                        MaSoThue = "",
                        DiaChiCongTy = ""

                    };
                    _accMember298Repository.Add(khachHang);
                    _unitOfWork.Commit();
                    //new MailersController().GoiMailDangKy(luuVao).Deliver();
                    SetCookieLogin(this.Request.RequestContext, luuVao.Email);//setcookie de luu vao
                                                                              //lay toan bo thong tin cua user lưu vào cookie để load lên những form nào có thông tin như vậy

                    CookieHelper cookieHelper = new CookieHelper("User_Infor");
                    string jsonString = JsonConvert.SerializeObject(new KhachHangModel
                    {
                        Email = khachHang.Email,
                        Phone = khachHang.Phone,
                    });
                    cookieHelper.SetUserInforCookie(jsonString);
                    ViewData["Message"] = "Chúc mừng bạn đã đăng ký thành công!";
                    return View("dangky", luuVao);
                }
                else//khong rong thi ton tai 
                {
                    ViewData["Message"] = "Email này đã tồn tại";
                    return View("dangky", luuVao);
                }
            }
            else//!modelstale.isvalid
            {
                ViewData["Message"] = "Vui lòng nhập đầy đủ thông tin";
                return View("dangky", luuVao);
            }
        }
    }
}
