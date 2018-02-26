using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class VeChungToiModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tiêu đề.")]
        public string NameProduct { get; set; }
        public string Content { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập vị trí.")]
        public int sPosition { get; set; }
    }
}