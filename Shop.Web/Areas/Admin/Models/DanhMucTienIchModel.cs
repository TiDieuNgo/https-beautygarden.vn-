using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class DanhMucTienIchModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên danh mục.")]
        public string TenDanhMuc { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập icon danh mục.")]
        public string IconDanhMuc { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập vị trí xuất hiện danh mục")]
        public int SapXep { get; set; }
    }
}