using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Shop.Model
{
    [Table("Menu")]
    public class Menu
    {
        [Key]
        public int id_ { get; set; }
        public int idControl { get; set; }
        public bool VIP { get; set; }
        public string Menu2 { get; set; }
        public string MenuAdwords { get; set; }
        public string Link { get; set; }
        public int LevelMenu { get; set; }
        public string LinkHttp { get; set; }
        public string LinkHttp1 { get; set; }
       // [Required(ErrorMessage = "Bạn vui lòng nhập mô tả cho sản phẩm.")]
        public string Note { get; set; }
        //[ConfigurationPropertyAttribute("maxRequestLength", DefaultValue = 1024)]
        public string Img { get; set; }
        public string Style { get; set; }
        public string ui { get; set; }
        public string ContentLabel { get; set; }
        public string Content { get; set; }
        public bool Option { get; set; }
        public string ContentLabel1 { get; set; }

        public string Content1 { get; set; }
        public bool Option1 { get; set; }
        public string ContentLabel2 { get; set; }
        public string Content2 { get; set; }
        public bool Option2 { get; set; }
        public string ContentLabel3 { get; set; }
        public string Content3 { get; set; }
        public bool Option3 { get; set; }
        public string ContentLabel4 { get; set; }
        public string Content4 { get; set; }
        public bool Option4 { get; set; }
        public bool Option5 { get; set; }
        public bool Option6 { get; set; }
        public bool Option7 { get; set; }
        public bool Option8 { get; set; }
        public int sPosition { get; set; }

        public int IdNhaCungCap { get; set; }
        public int Visitor { get; set; }
        public bool ok { get; set; }
        public DateTime sDate { get; set; }
        public DateTime sDateOk { get; set; }
        public int idUser { get; set; }
        public int idUserOk { get; set; }
        public string SEOKeyWord { get; set; }

        public string SEODescription { get; set; }
        public int NumberHaveGift { get; set; }
        public int idMPADSys { get; set; }
        public int PriceOffPro { get; set; }
        public bool Option9 { get; set; }
        public string CodeProduct { get; set; }
        public int PricePromotion { get; set; }
        public bool HasProduct { get; set; }
        public bool HasMenuoption { get; set; }


        public bool HasSale { get; set; }
        public int SalePercent { get; set; }
       // [Required(ErrorMessage = "Bạn vui lòng nhập mã vạch cho sản phẩm")]
        public string BarCode { get; set; }
        public short BarcodeType { get; set; }
        public string Status { get; set; }

        public bool HasValue { get; set; }
        public bool HasOnHand { get; set; }
        public int? PricePro { get; set; }//khong set gia tri not null
        //[Required(ErrorMessage = "Bạn vui lòng nhập tên sản phẩm.")]
        public string NameProduct { get; set; }
        public string IconMenu { get; set; }//hinh truoc danh muc trong menu
        public int SoLanXem { get; set; }
        public bool ShowMenuTop { get; set; }
        public bool ShowMenuHome { get; set; }
        public int SapxepDanhMuc { get; set; }
        //[DefaultValue(1000)] 
        public string BannerDanhMuc { get; set; }
        public int SapXepSanPham { get; set; }
        public string ContentLabelTaiSao { get; set; }
        public string ContentTaiSao { get; set; }
        public string ContentTheoSp { get; set; }
        public string MoTaDanhMuc { get; set; }
        [DefaultValue(0)] 
        public int GiaThiTruong { get; set; }
        //public DateTime NgayHetHang { get; set; } //    không dựa vào onhand nữa, dựa vào ngày hết hàng
        public bool GiaHot { get; set; }
        public bool DungSai { get; set; }
        public string NameProductLong { get; set; }
        public string TenNgan { get; set; }
        public string MoTaNgan { get; set; }
        public string SEOtitle { get; set; }

        //ngày hết hàng mới thêm vào
        public DateTime NgayHetHang { get; set; }
        public string NguoiTao { get; set; }

        //show top 100 ra ngoài màn hình bằng cách check
        public bool Bestseller { get; set; }
    }
    public enum BarcodeType:short
    {
        KhongCoGi=0,
        Mau=1,
        Mui=2
    }
    public class MenuMapping
    {
        public int id_ { get; set; }
        public int IdMenu { get; set; }//DanhMucTinTucId
        public string TenSanPham { get; set; }
    }

    public class MenuCategoryTree
    {
       
        public int DanhMucId { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public int ParentId { get; set; }
        public int Level { get; set; }
        public int SapXepDanhMuc { get; set; }
    }

    public class MenuCategoryForAddPromotionmapping
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Price { get; set; }
        public int? PriceDiscount { get; set; }


    }

    public class ProductFrontPageMapping
    {
        public class ProductForViewDetail
        {
            public int ProductId { get; set; }
            public string Link { get; set; }
            public string Note { get; set; }
            public string Img { get; set; }
            public string ContentLabel { get; set; }
            public string ContentLabel1 { get; set; }
            public string Content { get; set; }
            public string Content1 { get; set; }
            public string ContentLabel2 { get; set; }
            public string Content2 { get; set; }
            public bool Option2 { get; set; }
            public string ContentLabel3 { get; set; }
            public string Content3 { get; set; }
            public string ContentLabel4 { get; set; }
            public string Content4 { get; set; }
            public int IdDanhMuc { get; set; }
            public int BrandId { get; set; }
            public string BrandName { get; set; }
            public bool HasPromotion { get; set; }
            public decimal PricePromotion { get; set; }
            public short Discount { get; set; }
            public int IdNhaCungCap { get; set; }
            public string SEOKeyWord { get; set; }
            public string SEODescription { get; set; }
            public string BarCode { get; set; }
            public short BarcodeType { get; set; }
            public Int32? PricePro { get; set; }
            public int Price { get { return PricePro.HasValue ? PricePro.Value : 0; } }
            public string NameProduct { get; set; }
            public string ContentTaiSao { get; set; }
            public string ContentLabelTaiSao { get; set; }
            public string ContentTheoSp { get; set; }
            public int GiaThiTruong { get; set; }
            public string NameProductLong { get; set; }
            public string SEOtitle { get; set; }
            //ngày hết hàng mới thêm vào
            public DateTime NgayHetHang { get; set; }
            public bool HasOnHand { get; set; }
        }

        public class ProductShowCart
        {
            public int ProductId { get; set; }
            public string Img { get; set; }
            public Int32? PricePro { get; set; }
            public int Price { get { return PricePro.HasValue ? PricePro.Value : 0; } }
            public string Link { get; set; }
            public string NameProduct { get; set; }
            public string Barcode { get; set; }
            public string AttributeImg { get; set; }
            public string AttributeName { get; set; }
            public int AttributeFlag { get; set; }
         
        }

        public class Brand
        {
            public string Name { get; set; }
        }

        public class Attribute
        {
            public int id_ { get; set; }
            public string Barcode { get; set; }
            public string Img { get; set; }
            public string Name { get; set; }
            public int Flag { get; set; }
            public int ProductId { get; set; }
            public bool Selected { get; set; }
        }


        public class ProductBox
        {
            public int id_ { get; set; }
            public string Img { get; set; }
            public string NameProduct { get; set; }
            public Int32? PricePro { get; set; }
            public int Price { get { return PricePro.HasValue ? PricePro.Value : 0; } }
            public bool HasPromotion { get; set; }
            public int PricePromotion { get; set; }
            public short Discount { get; set; }
            public string Link { get; set; }
            public DateTime sDate { get; set; }
            public string NameProductLong { get; set; }
            public string Note { get; set; }
            //ngày hết hàng mới thêm vào
            public DateTime NgayHetHang { get; set; }
            public bool HasOnHand { get; set; }
            public bool Bestseller { get; set; }

        }

        public class ProductBoxDungdeptrai
        {
            public int id_ { get; set; }
            public string Img { get; set; }
            public string NameProduct { get; set; }
            public Int32? PricePro { get; set; }
            public int Price { get { return PricePro.HasValue ? PricePro.Value : 0; } }
            public bool HasPromotion { get; set; }
            public int PricePromotion { get; set; }
            public short Discount { get; set; }
            public string Link { get; set; }
            public DateTime sDate { get; set; }
        }
        public class ThuongHieuNoiBat
        {
            public int id_ { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public string Link { get; set; }
        }

        public class CategoryBoxHome
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Link { get; set; }
            public string Icon { get; set; }
            public IList<CategoryBoxHomeChild> Childs { get; set; }
        }
        public class CategoryBoxHomeChild
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Link { get; set; }
            public IList<ProductFrontPageMapping.ProductShow> Products { get; set; } 
        }
        public class ProductShow
        {
            public int ProductId { get; set; }
            public string Link { get; set; }
            public string Img { get; set; }

            public bool HasPromotion { get; set; }
            public decimal PricePromotion { get; set; }
            public short Discount { get; set; }

            public Int32? PricePro { get; set; }
            public int Price { get { return PricePro.HasValue ? PricePro.Value : 0; } }
            public DateTime sDate { get; set; }
            public string NameProduct { get; set; }
            public int SoLanXem { get; set; }
            public bool Option1 { get; set; }
            //mới thêm
            public DateTime NgayHetHang { get; set; }
            public bool HasOnHand { get; set; }

        }

        public class CategoryBoxHomeLv1
        {
            public int id_ { get; set; }
            public string CategoryName { get; set; }
            public int idControl { get; set; }
            public int LevelMenu { get; set; }
            public string Link { get; set; }
            public string IconMenu { get; set; }
            public bool ShowMenuHome { get; set; }
            public int SapxepDanhMuc { get; set; }
            public bool ok { get; set; }
        }

        public class BannerShowHome
        {
            public string Link { get; set; }
            public string Name { get; set; }
            public string Img { get; set; }
            public bool ok { get; set; }
        }

        public class BrandMapping
        {
            public int Id { get; set; }
            public string Link { get; set; }
            public string Name { get; set; }
            public string Img { get; set; }
            public int idControl { get; set; }
            public bool DungSai { get; set; }
            public bool ok { get; set; }
        }
    }

    public class ProductHelper
    {
        public static ProductFrontPageMapping.ProductBox CheckPromotion(ProductFrontPageMapping.ProductBox product,IList<PromotionMapping.PromotionCheckProduct> promotions )
        {
            foreach (var pr in promotions)
            {
                var promotionDetail = pr.PromotionDetails.FirstOrDefault(dt => dt.ProductId == product.id_);
                if (promotionDetail != null)
                {
                    product.HasPromotion = true;
                    product.PricePromotion = (int)promotionDetail.PriceDiscount;
                    product.Discount = (short)promotionDetail.Percent;
                    break;
                }
            }
            return product;
        }
    }
}
