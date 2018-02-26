using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.Helper;
using Shop.Web.Models;


namespace Shop.Web.Controllers
{
    public class LienHeController : Controller
    {
        private readonly IDetailMenuCommentRepository _detailMenuCommentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LienHeController(IDetailMenuCommentRepository detailMenuCommentRepository, IUnitOfWork unitOfWork)
        {
            _detailMenuCommentRepository = detailMenuCommentRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            CookieHelper cookieHelper = new CookieHelper("User_Infor");

            LienHeModel model = new LienHeModel();
            if (cookieHelper.GetCookie() != null)
            {

                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["User_Infor"]);
                if (!string.IsNullOrEmpty(json))
                {
                    LienHeModel form = JsonConvert.DeserializeObject<LienHeModel>(json);
                    model.Name = form.Name;//ho ten
                    model.HuongDanSuDung = form.HuongDanSuDung; //Email
                    model.Link = form.Link;//sdt
                    model.GiaoHang = form.GiaoHang;//dia chi
                }

            }
            return View("LienHe", model);
        }
        public ActionResult Submit()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Submit(LienHeModel luuVao)
        {
            if (!ModelState.IsValid)
            {
                ViewData["message"] = "Vui lòng nhập đầy đủ thông tin!";
                return View("LienHe");
            }
            DetailMenuComment lienHe = new DetailMenuComment()
            {
                id_Menu = 7777,
                sDate = DateTime.Now,
                sDateOk = DateTime.Now,
                idControl = 0,
                Ma_Hang = "",
                Name = luuVao.Name,
                Number = 0,
                ok = false,
                ShowIconHot = false,
                ShowIconNew = false,
                ShowMenu = false,
                ShowKhuyenMai = false,
                TinhTrangSP = false,
                idUser = 15,
                idUserOk = 15,
                GiaoHang = luuVao.GiaoHang,
                HuongDanSuDung = luuVao.HuongDanSuDung,
                Link = luuVao.Link,
                Content = luuVao.Content,
                Img = "",
                sPosition = 0,
                BanHangChuan = "",
                BaoHanh = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36",
                Brochure = "Desktop",
                Code = "Liên hệ",
                Diagram = "171.232.236.109",
                LinkHTTP = "/lien-he.html",
                Link02 = "https://beautygarden.vn/lien-he.html",


            };
            _detailMenuCommentRepository.Add(lienHe);
            _unitOfWork.Commit();
            //new MailersController().GoiMailLienHe(luuVao).Deliver();
            ViewData["thongbao"] = "Thông tin của bạn đã được ghi nhận, bộ phận Tư Vấn sẽ sớm liên hệ với bạn!";
            CookieHelper cookieHelper = new CookieHelper("User_Infor");
            string jsonString = JsonConvert.SerializeObject(new LienHeModel()
            {
                HuongDanSuDung = lienHe.HuongDanSuDung,
                Link = lienHe.Link,
                Name = lienHe.Name,
                GiaoHang = lienHe.GiaoHang
            });
            cookieHelper.SetUserInforCookie(jsonString);
            return View("LienHe");
        }
    }
}
