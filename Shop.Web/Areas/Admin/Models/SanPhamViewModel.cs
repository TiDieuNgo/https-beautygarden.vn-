using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Shop.Model;

namespace Shop.Web.Areas.Admin.Models
{
    public class SanPhamViewModel
    {
        public IList<SelectListItem> DanhMucs { get; set; }
        public int DanhMucId { get; set; }
        public PagedList.IPagedList<Menu> Menus { get; set; }
        public int soluongtimduoc { get; set; }
        public SaveProductFormModel SaveProduct { get; set; }
        public int stt { get; set; }
        public int page { get; set; }
       
    }

    public class ProductGiftSelect
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public class SaveProductFormModel
    {
        public int id_ { get; set; }
        //them san pham
        public short BarcodeType { get; set; }
        public int idControl { get; set; }
        public bool VIP { get; set; }
        public string Menu2 { get; set; }
        public string MenuAdwords { get; set; }
        public string Link { get; set; }
        public int LevelMenu { get; set; }
        public string LinkHttp { get; set; }
        public string LinkHttp1 { get; set; }
        //[Required(ErrorMessage = "Bạn vui lòng nhập mô tả cho sản phẩm.")]
        public string Note { get; set; }
        //[Required(ErrorMessage = "Bạn vui lòng nhập hình đại diện sản phẩm")]
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
        public string BarCode { get; set; }
        public string Status { get; set; }
        public bool Flag { get; set; }
        public bool HasValue { get; set; }
        public bool HasOnHand { get; set; }
        public string NameProduct { get; set; }

        //3 class duoi dung de luu tam hình anh, danh mục, ma vạch trước khi submit vào db
        //hinh anh khac
        public string OtherImages { get; set; }
        //them danh muc
        public string DanhMucIds { get; set; }
        //them ma vach
        public string MaVachJson { get; set; }
        //them ma vach mui
        public string MaVachMuiJson { get; set; }
        //them taglist
        public string Taglist { get; set; }
        public int SapxepDanhMuc { get; set; }
        public int SapXepSanPham { get; set; }
        public string ContentTaiSao { get; set; }
        public string ContentLabelTaiSao { get; set; }
        public string ContentTheoSp { get; set; }
        public int GiaThiTruong { get; set; }
        public bool GiaHot { get; set; }
        public string mySingleField { get; set; }
        public string NameProductLong { get; set; }
        public string SEOtitle { get; set; }

        //kho qua tang
        public string IdSanPhamQuaTang { get; set; }
        public IList<SelectListItem> SanPhams { get; set; }
        public string IdQUaTangs { get; set; }
    }

    public class SanPhamIdsModel
    {
        public string ids { get; set; }
    }
}