using System;
using System.Collections.Generic;
using System.Linq;
using Shop.Data;
using Shop.Model;

namespace Shop.Web.Model
{
    public class SanPhamModel
    {

    }
    public class ChiTietModel
    {
        public IList<Menu> DanhMucs { get; set; }
        public Menu SanPham { get; set; }
        public IList<SanPhamTheoDanhMuc> SanPhamMuaCung { get; set; }
        public IList<SanPhamTheoDanhMuc> SanPhamCungLoai { get; set; }
        public IList<MenuOption> MenuOptions { get; set; }
        public IList<Menu> ThuongHieu { get; set; }
        public int FlagMauMui { get; set; }
        //kiem tra ma san pham do con nam trong bang menuoption hay khong
        public int CountMaVach { get; set; }
        //kiem tra san pham do con hang hay het hang dua vao id san pham
        public PromotionMapping.PromotionCheck Promotion { get; set; }
    }

    public class SanPhamTheoDanhMuc
    {
        public int id_ { get; set; }
        public int LevelMenu { get; set; }
        public bool ok { get; set; }
        public DateTime sDate { get; set; }
        public int PriceOffPro { get; set; }
        public bool HasValue { get; set; }
        public bool HasOnHand { get; set; }
        public Int32? PricePro { get; set; }//khong set gia tri not null
        public Int32 Price { get { return PricePro.HasValue ? PricePro.Value : 0; } }
        public string NameProduct { get; set; }
        public string IconMenu { get; set; }//hinh truoc danh muc trong menu
        public string Img { get; set; }
        public int idControl { get; set; }
        public string Link { get; set; }
        public bool HasPromotion { get; set; }
        public decimal PricePromotion { get; set; }
        public short Discount { get; set; }
    }
    public class DanhSach
    {
        public Menu DanhMuc { get; set; }
        public PagedList.IPagedList<ProductFrontPageMapping.ProductBox> SanPhamTheoDanhMucs { get; set; }
        public IList<ProductFrontPageMapping.ProductBox> SanPhamTheoTags { get; set; }
        public int soluongtimduoc { get; set; }
        public PromotionMapping.PromotionCheck Promotion { get; set; }
        public IList<ProductFrontPageMapping.ProductBox> DanhSachYeuThichs { get; set; }
        public int page { get; set; }
        public DanhSachTag DanhSachTag { get; set; }
        public static int GetslHientai(int id)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("select COUNT(*) from BannerDanhSachs where IdDanhMuc={0}", id);
                return context.Database.SqlQuery<int>(sql).First();
            }
        }
        public static string GetslHinhAnhThuongHieu(string link)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("select MenuAdwords from Menu where Menu.Style='thuong-hieu' and Menu.Link='{0}'", link);
                return context.Database.SqlQuery<string>(sql).First();
            }
        }
        public static string GetBannerByDanhMuc(int id)
        {
            using (var context = new ShopDataContex())
            {
                string sql = string.Format("select MenuAdwords from Menu where Menu.Style='thuong-hieu' and Menu.id_={0}", id);
                return context.Database.SqlQuery<string>(sql).FirstOrDefault();
            }
        }

    }
    public class XemDanhMuc
    {
        public Menu DanhMuc { get; set; }
        public IList<SanPhamTheoDanhMuc> SanPhamTheoDanhMucs { get; set; }
    }
    public class BanChayModel
    {
        public IList<ProductSlishow> ProductSlishows { get; set; }
        public PromotionMapping.PromotionCheck KhuyenMai { get; set; }
    }
    public class ProductSlishow
    {
        public IList<ProductFrontPageMapping.ProductBox> Products { get; set; }
    }
    public class SanPhamMoiVeModel
    {
        public IList<ProductFrontPageMapping.ProductBox> Products { get; set; }
        public PromotionMapping.PromotionCheck KhuyenMai { get; set; }
    }
    public class SanPhamMoiVeThuocDanhMuc
    {
        public IList<ProductFrontPageMapping.ProductBox> Products { get; set; }
        public PromotionMapping.PromotionCheck KhuyenMai { get; set; }
    }
    public class SanPhamMuaCungModel
    {
        public IList<ProductFrontPageMapping.ProductBox> Products { get; set; }
        public PromotionMapping.PromotionCheck KhuyenMai { get; set; }
    }
    public class SanPhamGiaTotMoiNgayModel
    {
        public IList<ProductFrontPageMapping.ProductBoxDungdeptrai> Products { get; set; }
        public PromotionMapping.PromotionCheck KhuyenMai { get; set; }
    }
    public class SanPhamCungXemModel
    {
        public IList<ProductFrontPageMapping.ProductBox> Products { get; set; }
        public PromotionMapping.PromotionCheck KhuyenMai { get; set; }
    }
    public class DanhSachBestSeller
    {
        public Menu DanhMuc { get; set; }
        public PagedList.IPagedList<ProductFrontPageMapping.ProductBox> SanPhamTheoDanhMucs { get; set; }
        public int page { get; set; }
    }
    public class KhoQuaTangModel
    {
        public PagedList.IPagedList<ProductFrontPageMapping.ProductBox> Products { get; set; }
        public PromotionMapping.PromotionCheck KhuyenMai { get; set; }
    }
    public class QuaTangModel
    {
        public IList<ProductFrontPageMapping.ProductBox> Products { get; set; }
        public PromotionMapping.PromotionCheck KhuyenMai { get; set; }
    }
}