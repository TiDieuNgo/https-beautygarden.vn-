using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class ThemThuongHieuModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên thương hiệu.")]
        public string NameProduct { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập hình đại diện thương hiệu.")]
        public string Img { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập vị trí.")]
        public int sPosition { get; set; }
        public string Note { get; set; }
        public string LinkHttp1 { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập Title SEO")]
        public string SEOtitle { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập Dcription SEO")]
        public string SEODescription { get; set; }
    }
}