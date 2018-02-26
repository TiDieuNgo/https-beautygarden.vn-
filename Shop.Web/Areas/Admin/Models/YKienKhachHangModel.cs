using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class YKienKhachHangModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên thương hiệu.")]
        public string TenKhachHang { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập hình đại diện thương hiệu.")]
        public string HinhDaiDien { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập mô tả bài viết.")]
        public string MoTa { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập link facebook.")]
        public string LinkFaceBook { get; set; }
        public string NoiDung { get; set; }
    }
}