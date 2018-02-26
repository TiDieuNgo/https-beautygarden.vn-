using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Shop.Data;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Common;
using Shop.Web.Areas.Admin.Models;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.Authentication;
using Shop.Web.Model;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class DangNhapController : BaseController
    {
        private readonly IAccount298Repository _account298Repository;
        private readonly IFormsAuthentication formAuthentication;
        static readonly ISitemapNodeRepository repositorySitemap = new SitemapNodeRepository();
        static readonly IDatafeedNodeRepository repositoryDatafeed = new DatafeedNodeRepository();
        static readonly IShoppingAdsNodeRepository repositoryShoppingAds = new ShoppingAdsNodeRepository();
        public DangNhapController(IAccount298Repository account298Repository, IFormsAuthentication formAuthentication)
        {
            _account298Repository = account298Repository;
            this.formAuthentication = formAuthentication;
        }

        private bool ValidatePassword(Account298 user, string password)
        {
            var encoded = Md5Encrypt.Md5EncryptPassword(password);
            return user.Password.Equals(encoded);
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("Index");
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(DangNhapModel form, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                Account298 user = _account298Repository.Get(o => o.Username.Equals(form.Username));
                if (user != null)
                {
                    if (ValidatePassword(user, form.Password))
                    {
                        //dang nhap thanh cong
                        //  SetCookieLogin(this.Request.RequestContext, form.Username);
                        formAuthentication.SetAuthCookie(this.HttpContext, UserAuthenticationTicketBuilder.CreateAuthenticationTicket(user));
                        return RedirectToAction("ViewDanhMuc", "DanhMucSanPham");
                    }
                    else
                    {
                        ViewData["Message"] = "Mật Khẩu Sai";
                        return View("Index", form);
                    }
                }
                else
                {
                    ViewData["Message"] = "Tên đăng nhập không tồn tại";
                    return View("Index", form);
                }
            }
            else
            {
                return View("Index", form);
            }

        }
        public ActionResult LogOff()
        {
            formAuthentication.Signout();
            return RedirectToAction("Login", "DangNhap");
        }
        public ActionResult GetUser()
        {
            string username = User.Identity.Name;
            Account298 ac = null;
            if (!string.IsNullOrEmpty(username))
            {
                ac = _account298Repository.Get(o => o.Username.Equals(username));
                return View("User", ac);
            }
            return View("User", ac);
        }
        [HttpPost]
        public void ExportToCSVAdsDynamicGoogle(ThemTinTucModel obj)
        {
            try
            {
                string tacvu = obj.Content;
                tacvu = string.IsNullOrEmpty(tacvu) ? "" : tacvu;
                StringWriter sw = new StringWriter();
                sw.WriteLine("ID\t" +
                             "ID2\t" +
                             "Item title\t" +
                             "Final URL\t" +
                             "Image URL\t" +
                             "Item subtitle\t" +
                             "Item description\t" +
                             "Item category\t" +
                             "Price\t" +
                             "Sale price\t" +
                             "Contextual keywords\t" +
                             "Item address\t" +
                             "Tracking template\t" +
                             "Custom parameter\t" +
                             tacvu);

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=CSVAdsDynamicGoogle.csv");
                Response.ContentType = "application/octet-stream";

                ShopDataContex db = new ShopDataContex();
                var Menus = db.Menu.Where(o => o.idControl == 11 && o.ok && o.HasValue && o.HasOnHand && o.DungSai && o.TenNgan != null && o.MoTaNgan != null).OrderByDescending(o => o.sDateOk).ToList();

                foreach (var menu in Menus)
                {
                    sw.WriteLine(string.Format("{0}\t" +
                                               "{1}\t" +
                                               "{2}\t" +
                                               "{3}\t" +
                                               "{4}\t" +
                                               "{5}\t" +
                                               "{6}\t" +
                                               "{7}\t" +
                                               "{8}\t" +
                                               "{9}\t" +
                                               "{10}\t" +
                                               "{11}\t" +
                                               "{12}\t" +
                                               "{13}\t"+
                                               "{14}",
                        "BG" + menu.id_,
                        "",
                        CheckString25Char(menu.TenNgan == null ? menu.NameProductLong : menu.TenNgan),
                        "https://beautygarden.vn/" + menu.Link + ".html",
                        "https://beautygarden.vn/Upload/Files/" + menu.Img,
                        "beautygarden.vn",
                        CheckString25Char(menu.MoTaNgan == null ? "Mỹ Phẩm Chính Hãng" : menu.MoTaNgan),
                        GetTenDanhMuc(menu.id_),
                        menu.PricePro + " VND",
                        "",
                        "",
                        "",
                        "",
                        "",
                        tacvu
                    ));

                }
                Response.Write(sw.ToString());
                Response.End();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        protected string CheckString25Char(string Str)
        {
            Str = Str + string.Empty;
            Str = HttpUtility.HtmlDecode(Str.Trim().Replace("\t", ""));

            if (Str.Length <= 25) return Str;

            return Str.Remove(25);
        }
        public static string GetTenDanhMuc(int idsanpham)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("select top 1 NameProductLong from Menu where Menu.id_=(select top 1 idMenuCatelogy from MenuProAdd where idMenuProAdded={0})", idsanpham);
                return context.Database.SqlQuery<string>(sql).FirstOrDefault();
            }
        }
        public string GetImageSiteMapSEO(object content, string siteRoot)
        {
            StringBuilder rss = new StringBuilder();
            try
            {
                string temp = HttpUtility.HtmlDecode(content + string.Empty);
                if (temp.IndexOf(".jpg") > 0 || temp.IndexOf(".png") > 0 || temp.IndexOf(".gif") > 0 || temp.IndexOf(".jpeg") > 0)
                {
                    foreach (Match match in Regex.Matches(temp, "<img (.*?) />"))
                    {
                        Match matchSrc = Regex.Match(match.Value, "src=\"(.*?)\"");
                        Match matchAlt = Regex.Match(match.Value, "alt=\"(.*?)\"");
                        Match matchTitle = Regex.Match(match.Value, "title=\"(.*?)\"");
                        string imgSrc = matchSrc.Value.Replace("src=", "").Replace("https://beautygarden.vn/", "").Replace("\"", "");
                        if (imgSrc.StartsWith("/"))
                        {
                            string imgSrcTemp = Server.MapPath(imgSrc);

                            // if (System.IO.File.Exists(imgSrcTemp))
                            //{
                            // System.Drawing.Image img = System.Drawing.Image.FromFile(@imgSrcTemp);


                            rss.AppendLine("<image:image>");
                            rss.AppendLine("<image:loc>" + siteRoot.TrimEnd('/') + imgSrc + "</image:loc>");
                            if (matchTitle.Value.Trim() != string.Empty) rss.AppendLine("<image:title><![CDATA[" + matchTitle.Value.Replace("title=\"", "").Replace("\"", "") + "]]></image:title>");
                            if (matchAlt.Value.Trim() != string.Empty) rss.AppendLine("<image:caption><![CDATA[" + matchAlt.Value.Replace("alt=\"", "").Replace("\"", "") + "]]></image:caption>");
                            rss.AppendLine("</image:image>");

                            //}
                        }
                    }
                }

            }

            catch (Exception ex) { }
            return rss.ToString();

        }
        //Ghi vào file XML
        protected void WriteFile(string strContent)
        {
            try
            {
                //Response.AddHeader("content-disposition", "attachment;filename=sitemap.xml");
                //Response.ContentType = "text/xml; charset=utf-8";
                string link = Server.MapPath("~/sitemap.xml");
                //if (System.IO.File.Exists(link));
                StreamWriter sw = new StreamWriter(link, true);
                //Response.Write(strContent);
                sw.Write(strContent);
                sw.Close();
            }
            catch (Exception)
            {
            }
        }
        public void exportDatafeed()
        {
            string xmldatafeed = repositoryDatafeed.SetDatafeedNodes(this.Url, HostingEnvironment.ApplicationPhysicalPath + "datafeed.xml");
        }

        public void Exportsitemap()
        {
            string xmlsitemap = repositorySitemap.SetSitemapNodes(this.Url, HostingEnvironment.ApplicationPhysicalPath + "sitemap.xml");
        }
        public void ExportShopinAdsCocCoc()
        {
            string xmlsitemap = repositoryShoppingAds.SetShoppingAdsNodes(this.Url, HostingEnvironment.ApplicationPhysicalPath + "shoppingAds.xml");
        }
        //[HttpPost]
        //public void GetDataShopinAdsCocCoc()
        //{
        //    try
        //    {
        //        string siteRoot = "https://beautygarden.vn/";
        //        string nameshop = "CÔNG TY CỔ PHẦN BEAUTY GARDEN";
        //        StringBuilder rss = new StringBuilder();
        //        rss.AppendLine("<urlset xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:image=\"http://www.google.com/schemas/sitemap-image/1.1\" xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\" xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
        //        // Url Site
        //        rss.AppendLine("<url><loc>" + siteRoot + "</loc></url>");
        //        IList<Menu> data = Shop.Web.Model.YKienKhachHangModel.GetAllLinkforsitemap();
        //        // Table Menu
        //        foreach (var sp in data)
        //        {
        //            string tempContent = "";
        //            if (sp.Style + string.Empty == "san-pham")
        //            {
        //                rss.AppendLine("<url>");
        //                rss.AppendLine("<loc>" + siteRoot + sp.Link + ".html" + "</loc>");
        //                rss.AppendLine("<lastmod>" + sp.sDate.ToString("yyyy-MM-dd") + "</lastmod>");
        //                tempContent = sp.Content + " " + sp.Content1 + " " + sp.Content2 + " " + sp.Content3 + " " + sp.Content4;
        //                rss.Append(GetImageSiteMapSEO(tempContent, siteRoot));
        //                rss.AppendLine("</url>");

        //            }
        //            else if (sp.Style + string.Empty == "thuong-hieu")
        //            {
        //                //https://beautygarden.vn/thuong-hieu/play-boy.html
        //                rss.AppendLine("<url>");
        //                rss.AppendLine("<loc>" + siteRoot + "thuong-hieu/" + sp.Link + ".html" + "</loc>");
        //                rss.AppendLine("<lastmod>" + sp.sDate.ToString("yyyy-MM-dd") + "</lastmod>");
        //                rss.AppendLine("</url>");
        //            }
        //            else if (sp.Style + string.Empty == "danh-muc-san-pham")
        //            {
        //                //http://beautygarden.vn/danh-muc/trang-diem.html
        //                rss.AppendLine("<url>");
        //                rss.AppendLine("<loc>" + siteRoot + "danh-muc/" + sp.Link + ".html" + "</loc>");
        //                rss.AppendLine("<lastmod>" + sp.sDate.ToString("yyyy-MM-dd") + "</lastmod>");
        //                rss.AppendLine("</url>");
        //            }

        //        }
        //        rss.AppendLine("</urlset>");
        //        WriteFile(rss.ToString());
        //    }
        //    catch (Exception ex) { }
        //}
    }
}
