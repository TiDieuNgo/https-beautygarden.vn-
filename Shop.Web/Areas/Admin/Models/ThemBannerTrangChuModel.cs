using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class ThemBannerTrangChuModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên danh mục.")]
        public string NameProduct { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập hình đại diện.")]
        public string Img { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập vị trí.")]
        public int sPosition { get; set; }
        public string Link { get; set; }
        public string LinkHttp1 { get; set; }
    }
    public class BannerTrangDanhSachModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên danh mục.")]
        public string TenBanner { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập hình đại diện.")]
        public string HinhAnh { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập vị trí.")]
        public int ViTri { get; set; }
        public string Link { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng chọn danh mục")]
        public int IdDanhMuc { get; set; }
        public string LinkHttp1 { get; set; }
        public bool BannerQCDanhSach { get; set; }
        public bool BannerQCChiTiet { get; set; }
    }
    public class ThemBannerKhuyenMaiModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên banner")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập hình đại diện.")]
        public string HinhAnh { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập mô tả")]
        public string MoTa { get; set; }
    }
}