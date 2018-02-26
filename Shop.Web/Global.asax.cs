using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Shop.Web.Core.Models;
using System.Security.Principal;
using Shop.Web.Core.Authentication;
using Shop.Web.Core.ActionFilters;

using System.Web.Http;
using Shop.Model;

namespace Shop.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserFilter());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // page error
            routes.MapRoute("Error404",
           "error404.html", new { controller = "Error", action = "PageNotFound" },
           new[] { "Shop.Web.Controllers" });

            //robots
            routes.MapRoute("Robots",
          "robots",
             new { controller = "Home", action = "Robots" },
          new[] { "Shop.Web.Controllers" });

            //tim kiem san pham
            //  "tags/{taglink}.html",
            routes.MapRoute("SanPhamTimKiem",
            "tim-kiem.html",
               new { controller = "SanPham", action = "TimKiem", id = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //tim kiem tin tuc
            routes.MapRoute("TinTucTimKiem",
                "tin-tuc.html",
                new { controller = "TinTuc", action = "TimKiem", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                new[] { "Shop.Web.Controllers" });

            //SuKienKhuyenMai
            routes.MapRoute("LienHe",
            "lien-he.html",
               new { controller = "LienHe", action = "Index", id = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //salepage
            routes.MapRoute("SalePageIndex",
                "sale-page.html",
                new { controller = "SalePage", action = "Index", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                new[] { "Shop.Web.Controllers" });

            //san pham da xem
            routes.MapRoute("SanPhamDaXem",
            "san-pham-ban-da-xem.html",
               new { controller = "SanPhamDaXem", action = "Index", id = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //Dang Nhap
            routes.MapRoute("DangNhap",
            "dang-nhap.html",
               new { controller = "DangNhap", action = "Index", id = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //danh sach yeu thich
            routes.MapRoute("DanhSachYeuThich",
            "danh-sach-yeu-thich.html",
            new { controller = "DanhSachYeuThich", action = "DanhSachYeuThich" },
            new[] { "Shop.Web.Controllers" });

            //Dang Nhap
            routes.MapRoute("DangKy",
            "dang-ky.html",
               new { controller = "DangKy", action = "Index", id = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //SuKienKhuyenMai
            routes.MapRoute("SuKienKhuyenMai",
            "su-kien-khuyen-mai.html",
               new { controller = "TinTuc", action = "SuKienKhuyenMai", id = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //BiQuyetLamDep
            routes.MapRoute("BiQuyetLamDep",
            "bi-quyet-lam-dep.html",
               new { controller = "TinTuc", action = "BiQuyetLamDep", id = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //Review san pham
            routes.MapRoute("Review",
            "review-san-pham",
               new { controller = "Review", action = "Index", id = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

           

            //gio hang
            routes.MapRoute("Cart",
            "gio-hang.html",
               new { controller = "Cart", action = "Index" },
            new[] { "Shop.Web.Controllers" });


            //san pham khuyen mai
            routes.MapRoute("Promotion",
           "san-pham-khuyen-mai.html",
              new { controller = "SanPham", action = "Promotion" },
           new[] { "Shop.Web.Controllers" });


            routes.MapRoute("Order",
            "thanh-toan-don-hang.html",
               new { controller = "Order", action = "Index" },
            new[] { "Shop.Web.Controllers" });

            routes.MapRoute("OrderSuccess",
           "dat-hang-thanh-cong-{id}.html",
              new { controller = "Order", action = "Success", id = UrlParameter.Optional },
           new[] { "Shop.Web.Controllers" });

            routes.MapRoute("ViewOrder",
             "DonHangDetail.aspx",
                new { controller = "Order", action = "ViewOrder", id = UrlParameter.Optional },
             new[] { "Shop.Web.Controllers" });

            //kho quà tặng
            routes.MapRoute("KhoQuaTang",
                "kho-qua-tang.html",
                new { controller = "KhoQuaTang", action = "Index", id = UrlParameter.Optional },
                new[] { "Shop.Web.Controllers" });


            //chi tiet san pham get he thong Bg.hvnet.vn
            routes.MapRoute("ChiTietTuHeThong",
            "sp.html",
               new { controller = "SanPham", action = "ChiTietTuHeThong", id = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //Tin bao chi
            routes.MapRoute("TinBaoChi",
            "bao-chi-noi-gi-ve-chung-toi.html",
               new { controller = "TinTuc", action = "TinBaoChi", id = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //chi tiet san pham
            routes.MapRoute("ChiTietSanPham",
          "{splink}.html",
             new { controller = "SanPham", action = "Detail", splink = UrlParameter.Optional, seo = UrlParameter.Optional },
          new[] { "Shop.Web.Controllers" });

            //chi tiet san pham amp
            routes.MapRoute("ChiTietSanPhamAMP",
          "{linkspamp}.amp",
             new { controller = "AMPSanPham", action = "Index", linkspamp = UrlParameter.Optional, seo = UrlParameter.Optional },
          new[] { "Shop.Web.Controllers" });

            //chi tiet tin tuc
            routes.MapRoute("ChiTietTinTuc",
            "tin-tuc/{seolink}.html",
               new { controller = "TinTuc", action = "ChiTiet", seolink = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //chi tiet tin tuc
            routes.MapRoute("ChiTietTinTucAMP",
            "tin-tuc/{linkttamp}.amp",
               new { controller = "AMPTinTuc", action = "Index", linkttamp = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //chi tiet review
            routes.MapRoute("ChiTietReview",
            "review/{seolink}.html",
               new { controller = "Review", action = "ChiTiet", seolink = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });


            //danh sach san pham
            routes.MapRoute("DanhSachSanPham",
            "danh-muc/{splink}.html",
               new { controller = "SanPham", action = "DanhSach", splink = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //danh sach tag sản phẩm
            routes.MapRoute("Tag",
            "tag/{tentag}.html",
               new { controller = "Tag", action = "DanhSach", tentag = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });

            //danh sach tag tin tức
            routes.MapRoute("TagTinTuc",
                "tags/{taglink}.html",
                new { controller = "Tag", action = "DanhSachtagForTinTuc", taglink = UrlParameter.Optional, seo = UrlParameter.Optional },
                new[] { "Shop.Web.Controllers" });

            //danh sach san pham thuoc thuong hieu
            routes.MapRoute("DanhSachSanPhamThuocThuongHieu",
            "thuong-hieu/{linkth}.html",
               new { controller = "ThuongHieu", action = "DanhSach", linkth = UrlParameter.Optional, seo = UrlParameter.Optional },
            new[] { "Shop.Web.Controllers" });
         
            //chi tiet san pham
            routes.MapRoute("NoiDungFooter",
          "noi-dung/{linkfooter}.html",
             new { controller = "NoiDungFooter", action = "ChiTiet", linkfooter = UrlParameter.Optional, seo = UrlParameter.Optional },
          new[] { "Shop.Web.Controllers" });

            //Hoi Dap San Pham
            routes.MapRoute("HoiDapSanPham",
                "{seo}-san-pham-{id}",
                new { controller = "HoiDapSanPham", action = "TraLoiCauHoi", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                new[] { "Shop.Web.Controllers" });

            //Hoi Dap San Pham cau hoi da co cau tra loi
            routes.MapRoute("CauHoiDaCoTraLoi",
                "{seo}-cau-hoi-{id}",
                new { controller = "HoiDapSanPham", action = "Allcauhoiocautraloi", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                new[] { "Shop.Web.Controllers" });

            //100 sp bán chạy
            routes.MapRoute("top100banchay",
                "bestseller/top-100-san-pham-ban-chay.html",
                new { controller = "BestSeller", action = "DanhSach100BanChay", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                new[] { "Shop.Web.Controllers" });


            //top 100 sản phẩm bán chạy
            routes.MapRoute("BestSeller",
                "{seo}-ban-chay-{id}",
                new { controller = "BestSeller", action = "DanhSach", id = UrlParameter.Optional, seo = UrlParameter.Optional },
                new[] { "Shop.Web.Controllers" });


            //salepage
            routes.MapRoute("SalePage",
                "sale-page/{linksalepage}.html",
                new { controller = "SalePage", action = "SalePageMoi", linksalepage = UrlParameter.Optional, seo = UrlParameter.Optional },
                new[] { "Shop.Web.Controllers" });


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                 namespaces: new string[] { "Shop.Web.Controllers" },
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
        //redirect 301
        //step 1: load toàn bộ link cũ lên
        //step 2: chạy vong for thay đổi link cũ thành link mới
        protected void Application_BeginRequest()
        {
            IList<Redirect301> allList= Shop.Web.Model.YKienKhachHangModel.GetLinkForredirect();
            if (allList.Any())
            {
                foreach (var redirect301 in allList)
                {

                    if (HttpContext.Current.Request.Url.ToString().ToLower().Contains(redirect301.LinkCu))
                    {
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.AddHeader("Location", Request.Url.ToString().ToLower().Replace(
                            redirect301.LinkCu,
                           redirect301.LinkMoi));
                    }
                }
            }
          

        }
        public override void Init()
        {
            this.PostAuthenticateRequest += this.PostAuthenticateRequestHandler;
            base.Init();
        }
        private void PostAuthenticateRequestHandler(object sender, EventArgs e)
        {
            HttpCookie authCookie = this.Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (IsValidAuthCookie(authCookie))
            {
                var formsAuthentication = DependencyResolver.Current.GetService<IFormsAuthentication>();

                var ticket = formsAuthentication.Decrypt(authCookie.Value);
                var ShopUser = new ShopUser(ticket);
                string[] userRoles = { ShopUser.RoleName };
                this.Context.User = new GenericPrincipal(ShopUser, userRoles);
                formsAuthentication.SetAuthCookie(this.Context, ticket);
            }
        }
        private static bool IsValidAuthCookie(HttpCookie authCookie)
        {
            return authCookie != null && !String.IsNullOrEmpty(authCookie.Value);
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleTable.Bundles.RegisterTemplateBundles();
            Bootstrapper.Run();
        }
      
    }
}