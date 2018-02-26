using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Shop.Model;
using Shop.Web.Core.Helper;

namespace Shop.Web.ViewModels
{
    public class CartViewModel
    {
        public class ProductCart
        {
            public int ProductId { get; set; }
            public string Barcode { get; set; }
            public int Quantity { get; set; }
        }

        public class CartItem
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string Link { get; set; }
            public string Image { get; set; }
            public int Price { get; set; }
            public int PricePromotion { get; set; }
            public bool HasPromotion { get; set; }
            public string Barcode { get; set; }
            public int Quantity { get; set; }
            public bool Available { get; set; }
            public short Discount { get; set; }
            public int Total { get { return Price * Quantity; } }

            public bool Iquatang { get; set; }

            public string AttributeImg { get; set; }
            public string AttributeName { get; set; }
            public int AttributeFlag { get; set; }
        }

        public class CartModel
        {
            public IList<CartItem> CartItems { get; set; }
            public int Total { get; set; }
            public bool Available { get; set; }
            public IList<int> ListProductId { get; set; } // lấy Id để truyền vào hàm remarketing
        }

        public class OrderModel
        {
            public IList<CartItem> CartItems { get; set; }
            public int Total { get; set; }
            public CartForm CartForm { get; set; }
            public DetailMenuComment Order { get; set; }
            public int orderIdForremarketting { get; set; } // lấy Id để truyền vào hàm remarketing
        }
        public class CartForm
        {
            [Required(ErrorMessage = "Nhập họ tên")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Nhập số điện thoại")]
            public string Phone { get; set; }
            public string Email { get; set; }
            [Required(ErrorMessage = "Nhập địa chỉ")]
            public string Address { get; set; }
           
            public string ShipAddress { get; set; }

            [Required(ErrorMessage = "Chưa chọn shop")]
            [Range(1, int.MaxValue, ErrorMessage = "Chưa chọn shop")]
            public int BrandId { get; set; }
            public string Note { get; set; }
            public string BrandName { get; set; }
            public int ProductId { get; set; }
            public string Barcode { get; set; }

            public CartForm()
            {
                CookieHelper cookieHelper = new CookieHelper("User_Infor");

                if (cookieHelper.GetCookie() != null)
                {
                    string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["User_Infor"]);
                    if (!string.IsNullOrEmpty(json))
                    {
                        Shop.Web.Models.LienHeModel form = JsonConvert.DeserializeObject<Shop.Web.Models.LienHeModel>(json);
                        Phone = form.Link;
                        Email = form.HuongDanSuDung;
                        Name = form.Name;
                        Address = form.GiaoHang;
                    }

                }
            }
        }
    }
}