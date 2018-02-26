using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Shop.Web.Model;

namespace Shop.Web.ShoppingAds
{
    public static class clsShoppingAds
    {
        public static string SetShoppingAds(UrlHelper urlHelper, string path)
        {
            string xml = GetDatafeedDocument(GetNode(urlHelper));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Save(path);
            return xml;
        }

        public static List<ShoppingAdsNode> GetNode(UrlHelper urlHelper)
        {
            List<ShoppingAdsNode> nodes = new List<ShoppingAdsNode>();
            string Domain = urlHelper.RequestContext.HttpContext.Request.Url.Scheme + "://" + urlHelper.RequestContext.HttpContext.Request.Url.Host;
            var Menus = Shop.Web.Model.YKienKhachHangModel.GetDataforFacebook();
            //danh sách sản phẩm
            foreach (var linksp in Menus)
            {
                nodes.Add(
                   new ShoppingAdsNode()
                   {
                      Productname = linksp.TenNgan,
                      UrlSp = "https://beautygarden.vn/" + linksp.Link +".html",
                      CompanyName = "Beauty Garden",
                      UrlHome = "https://beautygarden.vn",
                      images = "https://beautygarden.vn/Upload/Files/" + linksp.Img,
                      price = linksp.GiaThiTruong,
                      saleprice = linksp.PricePro, //giá sau khuyến mãi
                      keyword = RejectMarks(linksp.TenNgan)
                   });
            }
            return nodes;
        }
        public static string GetDatafeedDocument(List<ShoppingAdsNode> datafeedNodes)
        {
            XElement root = new XElement("Products");
            foreach (ShoppingAdsNode datafeedNode in datafeedNodes)
            {
                XElement urlElement = new XElement(
                     "product",
                    new XElement("name", datafeedNode.Productname),

                    datafeedNode.UrlSp == null ? null : new XElement(
                         "url",
                        datafeedNode.UrlSp),

                    new XElement(
                    "shop",
                    new XElement("name", datafeedNode.CompanyName),
               
                   datafeedNode.UrlHome == null ? null : new XElement(
                         "url",
                        datafeedNode.UrlHome)),

                   datafeedNode.images == null ? null : new XElement(
                         "image",
                        datafeedNode.images),

                 datafeedNode.price == null ? null : new XElement(
                         "price",
                        datafeedNode.price),

                  datafeedNode.saleprice == null ? null : new XElement(
                         "sale-price",
                        datafeedNode.saleprice),

                 datafeedNode.keyword == null ? null : new XElement(
                         "keyword",
                        datafeedNode.keyword));

                root.Add(urlElement);
            }

            XDocument document = new XDocument(root);
            return document.ToString();
        }
        public static string RejectMarks(string text)
        {
            text = text.ToLower();
            string[] pattern = new string[7];

            pattern[0] = "a|(á|ả|à|ạ|ã|ă|ắ|ẳ|ằ|ặ|ẵ|â|ấ|ẩ|ầ|ậ|ẫ)";

            pattern[1] = "o|(ó|ỏ|ò|ọ|õ|ô|ố|ổ|ồ|ộ|ỗ|ơ|ớ|ở|ờ|ợ|ỡ)";

            pattern[2] = "e|(é|è|ẻ|ẹ|ẽ|ê|ế|ề|ể|ệ|ễ)";

            pattern[3] = "u|(ú|ù|ủ|ụ|ũ|ư|ứ|ừ|ử|ự|ữ)";

            pattern[4] = "i|(í|ì|ỉ|ị|ĩ)";

            pattern[5] = "y|(ý|ỳ|ỷ|ỵ|ỹ)";

            pattern[6] = "d|đ";

            for (int i = 0; i < pattern.Length; i++)
            {
                // kí tự sẽ thay thế

                char replaceChar = pattern[i][0];

                MatchCollection matchs = Regex.Matches(text, pattern[i]);

                foreach (Match m in matchs)
                {
                    text = text.Replace(m.Value[0], replaceChar);
                }
            }
            text = Regex.Replace(text, @"[^\w\.@]", " ").Trim().ToLower();
            text = text.Replace(" ", " ");
            return text;
        }
    }
}