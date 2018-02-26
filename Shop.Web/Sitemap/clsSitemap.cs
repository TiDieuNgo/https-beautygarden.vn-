using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Shop.Web.Model;

namespace Shop.Web.Sitemap
{
    public static class clsSitemap
    {
        public static string SetSitemap(UrlHelper urlHelper, string path)
        {
            string xml = GetSitemapDocument(GetNode(urlHelper));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Save(path);
            return xml;
        }

        public static List<SitemapNode> GetNode(UrlHelper urlHelper)
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            string Domain = urlHelper.RequestContext.HttpContext.Request.Url.Scheme + "://" + urlHelper.RequestContext.HttpContext.Request.Url.Host;

            //trang chu
            nodes.Add(
                new SitemapNode()
                {
                    Url = Domain,
                    LastModified = DateTime.Now,
                    Frequency = SitemapFrequency.Daily,
                });
            //tin tuc: tin bao chi
            nodes.Add(
            new SitemapNode()
            {
                Url = Domain + urlHelper.RouteUrl("TinBaoChi"),
                LastModified = DateTime.Now,
                Frequency = SitemapFrequency.Daily,
            });
            //tin tuc: su kien khuyen mai
            nodes.Add(
                new SitemapNode()
                {
                    Url = Domain + urlHelper.RouteUrl("SuKienKhuyenMai"),
                    LastModified = DateTime.Now,
                    Frequency = SitemapFrequency.Daily,
                });
            //chi tiet su kien khuyen mai
            foreach (var linksp in Shop.Web.Model.YKienKhachHangModel.GetchitietSKKMs())
            {
                string urltam = Domain + urlHelper.Action(linksp.Link + ".html", "tin-tuc");
                nodes.Add(
                    new SitemapNode()
                    {
                        Url = urltam.Replace("/Admin", ""),
                        LastModified = DateTime.Now,
                        Frequency = SitemapFrequency.Daily,

                    });
            }
            //tin tuc: bi quyet lam dep
            nodes.Add(
                new SitemapNode()
                {
                    Url = Domain + urlHelper.RouteUrl("BiQuyetLamDep"),
                    LastModified = DateTime.Now,
                    Frequency = SitemapFrequency.Daily,
                });
            //chi tiet bi quyet lam dep
            foreach (var linksp in Shop.Web.Model.YKienKhachHangModel.GetchitietBQLDs())
            {
                string urltam = Domain + urlHelper.Action(linksp.Link + ".html", "tin-tuc");
                nodes.Add(
                    new SitemapNode()
                    {
                        Url = urltam.Replace("/Admin", ""),
                        LastModified = DateTime.Now,
                        Frequency = SitemapFrequency.Daily,

                    });
            }
            //tin tuc: review san pham
            nodes.Add(
                new SitemapNode()
                {
                    Url = Domain + urlHelper.RouteUrl("Review"),
                    LastModified = DateTime.Now,
                    Frequency = SitemapFrequency.Daily,
                });
            //chi tiet review san pham
            foreach (var linksp in Shop.Web.Model.YKienKhachHangModel.GetchitietReviewSPs())
            {
                string urltam = Domain + urlHelper.Action(linksp.Link + ".html", "review");
                nodes.Add(
                    new SitemapNode()
                    {
                        Url = urltam.Replace("/Admin", ""),
                        LastModified = DateTime.Now,
                        Frequency = SitemapFrequency.Daily,

                    });
            }
            //lien he
            nodes.Add(
            new SitemapNode()
            {
                Url = Domain + urlHelper.RouteUrl("LienHe"),
                LastModified = DateTime.Now,
                Frequency = SitemapFrequency.Daily,
            });
            //Giới Thiệu Beautygarden
            nodes.Add(
                new SitemapNode()
                {
                    Url = "https://beautygarden.vn/noi-dung/gioi-thieu-beautygarden.html",
                    LastModified = DateTime.Now,
                    Frequency = SitemapFrequency.Daily,
                });
            //Hệ Thống Cửa Hàng
            nodes.Add(
                new SitemapNode()
                {
                    Url = "https://beautygarden.vn/noi-dung/he-thong-cua-hang.html",
                    LastModified = DateTime.Now,
                    Frequency = SitemapFrequency.Daily,
                });
            //Chương Trình Tích Điểm
            nodes.Add(
                new SitemapNode()
                {
                    Url = "https://beautygarden.vn/noi-dung/chuong-trinh-tich-diem.html",
                    LastModified = DateTime.Now,
                    Frequency = SitemapFrequency.Daily,
                });
            //Bảo Mật Thông Tin
            nodes.Add(
                new SitemapNode()
                {
                    Url = "https://beautygarden.vn/noi-dung/bao-mat-thong-tin.html",
                    LastModified = DateTime.Now,
                    Frequency = SitemapFrequency.Daily,
                });
            //Thanh Toán & Vận Chuyển
            nodes.Add(
                new SitemapNode()
                {
                    Url = "https://beautygarden.vn/noi-dung/thanh-toan-van-chuyen.html",
                    LastModified = DateTime.Now,
                    Frequency = SitemapFrequency.Daily,
                });
            //Chính Sách Đổi Trả Hàng
            nodes.Add(
                new SitemapNode()
                {
                    Url = "https://beautygarden.vn/noi-dung/chinh-sach-doi-tra-hang.html",
                    LastModified = DateTime.Now,
                    Frequency = SitemapFrequency.Daily,
                });
            //danh sách danh mục
            foreach (var linksp in Shop.Web.Model.YKienKhachHangModel.GetDanhMucList())
            {
                string urltam = Domain + urlHelper.Action(linksp.Link + ".html", "danh-muc");
                nodes.Add(
                   new SitemapNode()
                   {
                       Url = urltam.Replace("/Admin",""),
                       LastModified = DateTime.Now,
                       Frequency = SitemapFrequency.Daily,
                      
                   });
            }
            //danh sách sản phẩm
            foreach (var linksp in Shop.Web.Model.YKienKhachHangModel.GetproductIdsList())
            {
                string Urltam = "";
                string urlsp = Domain + urlHelper.Action(linksp.Link + ".html","");
                Urltam = urlsp.Replace("/Admin", "");
                nodes.Add(
                   new SitemapNode()
                   {
                      
                       Url= Urltam.Replace("/Home",""),
                       LastModified = DateTime.Now,
                       Frequency = SitemapFrequency.Daily,
                   });
            }
            //danh sách thương hiệu
            foreach (var linksp in Shop.Web.Model.YKienKhachHangModel.GetThuongHieuList())
            {
                string urlth = Domain + urlHelper.Action(linksp.Link + ".html", "thuong-hieu");
                nodes.Add(
                   new SitemapNode()
                   {
                       Url = urlth.Replace("/Admin", ""),
                       LastModified = DateTime.Now,
                       Frequency = SitemapFrequency.Daily,
                   });
            }
            return nodes;
        }
        public static string GetSitemapDocument(List<SitemapNode> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");

            foreach (SitemapNode sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)),
                    sitemapNode.LastModified == null ? null : new XElement(
                        xmlns + "lastmod",
                        sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-dd")),
                    sitemapNode.Frequency == null ? null : new XElement(
                        xmlns + "changefreq",
                        sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
                    sitemapNode.Priority == null ? null : new XElement(
                        xmlns + "priority",
                        sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
                root.Add(urlElement);
            }

            XDocument document = new XDocument(root);
            return document.ToString();
        }

    }
}