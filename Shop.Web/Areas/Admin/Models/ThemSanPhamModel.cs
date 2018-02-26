using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class ThemSanPhamModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên sản phẩm.")]
        public string NameProduct { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập mô tả cho sản phẩm.")]
        public string Note { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập hình đại diện sản phẩm")]
        public string Img { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng chọn nhà cung cấp")]
        public string IdNhaCungCap { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập mã vạch cho sản phẩm")]
        public string BarCode { get; set; }
        public bool Option1 { get; set; }
        public bool Option5 { get; set; }
        public bool Option6 { get; set; }
        public int SapXepSanPham { get; set; }
        public string ContentLabel { get; set; }
        public string Content { get; set; }
        public string ContentLabel1 { get; set; }
        public string Content1 { get; set; }
        public string ContentLabel2 { get; set; }
        public string Content2 { get; set; }
        public string ContentLabel3 { get; set; }
        public string Content3 { get; set; }
        public string ContentLabel4 { get; set; }
        public string Content4 { get; set; }
    }
}