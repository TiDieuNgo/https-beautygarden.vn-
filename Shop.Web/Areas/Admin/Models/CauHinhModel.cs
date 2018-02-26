using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class CauHinhModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tiêu đề.")]
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
    }
}