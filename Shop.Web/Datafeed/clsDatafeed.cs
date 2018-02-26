using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Model;

namespace Shop.Web.Datafeed
{
    public static class clsDatafeed
    {
        
        public static string SetDatafeed(UrlHelper urlHelper, string path)
        {
            string xmldatafeed = GetDatafeedDocument(GetNode(urlHelper));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmldatafeed);
            doc.Save(path);
            return xmldatafeed;
        }

        public static List<DatafeedNode> GetNode(UrlHelper urlHelper)
        {
            List<DatafeedNode> nodes = new List<DatafeedNode>();
           // string Domain = urlHelper.RequestContext.HttpContext.Request.Url.Scheme + "://" + urlHelper.RequestContext.HttpContext.Request.Url.Host;

            //danh sách sản phẩm
            foreach (var sanpham in Shop.Web.Model.DatafeedNode.GetproductIdsListDatafeed())
            {
               
                nodes.Add(
                   new DatafeedNode()
                   {
                       Availability_instock = true,
                       Product_name = sanpham.Product_name,
                       Description =Regex.Replace(sanpham.Description, @"<[^>]+>|&nbsp;", "").Trim(),
                       Currency = "VND",
                       Price = sanpham.Price,
                       Discount = 0,//giam gia
                       Discounted_price = sanpham.Price,//gia con lai sau khi giam
                       Parent_of_cat_1 = sanpham.CategoryLv1,//danh muc level 1
                       Category_1 = sanpham.CategoryLv2,//danh muc hien tai
                       Picture_url = "https://beautygarden.vn/Upload/Files/"+sanpham.Picture_url,
                       Url = "https://beautygarden.vn/"+sanpham.Url +".html",
                       Promotion = "Mua từ 1 sản phẩm trở lên có quà tặng kèm!",
                       Delivery_period = "Từ 2 đến 5 ngày"
                   });
            }

            return nodes;
        }
        //public static string StripHTML(string input)
        //{
        //    return Regex.Replace(input, "<.*?>", String.Empty);
        //}
        public static string GetDatafeedDocument(List<DatafeedNode> datafeedNodes)
        {
            XElement root = new XElement("Products");

            foreach (DatafeedNode datafeedNode in datafeedNodes)
            {
                XElement urlElement = new XElement(
                     "Product",

                    new XElement("Availability_instock", datafeedNode.Availability_instock),

                    datafeedNode.Product_name == null ? null : new XElement(
                         "Product_name",
                        datafeedNode.Product_name),

                    datafeedNode.Description == null ? null : new XElement(
                         "Description",
                        datafeedNode.Description),

                   datafeedNode.Currency == null ? null : new XElement(
                         "Currency",
                        datafeedNode.Currency),

                   datafeedNode.Price == null ? null : new XElement(
                         "Price",
                        datafeedNode.Price),

                 datafeedNode.Discount == null ? null : new XElement(
                         "Discount",
                        datafeedNode.Discount),

                  datafeedNode.Discounted_price == null ? null : new XElement(
                         "Discounted_price",
                        datafeedNode.Discounted_price),

                 datafeedNode.Parent_of_cat_1 == null ? null : new XElement(
                         "Parent_of_cat_1",
                        datafeedNode.Parent_of_cat_1),

                  datafeedNode.Category_1 == null ? null : new XElement(
                         "Category_1",
                        datafeedNode.Category_1),

                 datafeedNode.Picture_url == null ? null : new XElement(
                         "Picture_url",
                        datafeedNode.Picture_url),

                 datafeedNode.Url == null ? null : new XElement(
                         "Url",
                        datafeedNode.Url),

                 datafeedNode.Promotion == null ? null : new XElement(
                         "Promotion",
                        datafeedNode.Promotion),

                    datafeedNode.Delivery_period == null ? null : new XElement(
                         "Delivery_period",
                        datafeedNode.Delivery_period));

                root.Add(urlElement);
            }

            XDocument document = new XDocument(root);
            return document.ToString();
        }

    }
}