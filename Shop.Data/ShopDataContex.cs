using System.Data.Entity;
using Shop.Model;

namespace Shop.Data
{
    public class ShopDataContex : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<TagsLink> TagsLink { get; set; }
        public DbSet<MenuOption> MenuOptions { get; set; }
        public DbSet<MenuImage> MenuImage { get; set; }
        public DbSet<AccMember298> AccMember298 { get; set; }
        public DbSet<DetailMenu> DetailMenu { get; set; }
        public DbSet<Account298> Account298 { get; set; }
        public DbSet<MenuProAdd> MenuProAdd { get; set; }
        public DbSet<DetailMenuComment> DetailMenuComment { get; set; }
        public DbSet<DetailMenuCommentItem> DetailMenuCommentItem { get; set; }
        public DbSet<ProductStockSync> ProductStockSync { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<BannerDanhSach> BannerDanhSaches { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionDetail> PromotionDetails { get; set; }
        public DbSet<DanhMucTienIch> DanhMucTienIches { get; set; }
        public DbSet<NoiDungTienIch> NoiDungTienIches { get; set; }
        public DbSet<YKienKhachHang> YKienKhachHangs { get; set; }
        public DbSet<ThietLap> ThietLaps { get; set; }
        public DbSet<TuKhoaTimKiem> TuKhoaTimKiems { get; set; }
        public DbSet<TuKhoaTapHop> TuKhoaTapHops { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<CauHinh> CauHinhs { get; set; }
        public DbSet<DanhSachTag> DanhSachTags { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<TraLoiComment> TraLoiComments { get; set; }
        public DbSet<Redirect301> Redirect301s { get; set; }
        public DbSet<SanPhamXemCung> SanPhamXemCungs { get; set; }
        public DbSet<BannerKhuyenMai> BannerKhuyenMais { get; set; }
        public DbSet<TagTinTuc> TagTinTucs { get; set; }
        public DbSet<HeThongCuaHang> HeThongCuaHangs { get; set; }
        public DbSet<SalePage> SalePages { get; set; }
        public DbSet<KhoQuaTang> KhoQuaTangs { get; set; }
        public DbSet<PopUpBannerKM> PopUpBannerKMs { get; set; }
        public DbSet<ThongBaoKhiCoHang> ThongBaoKhiCoHangs { get; set; }
        public DbSet<IframeSalepage> IframeSalepages { get; set; }
        public DbSet<SEOFooter> SEOFooters { get; set; }
        public virtual void Commit()
        {
            base.SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ShopDataContex>(null);
        }
       
    }
}
