using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class ThemDanhMucModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên danh mục.")]
        public string NameProduct { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập hình đại diện.")]
        public string Img { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập icon danh mục.")]
        public string IconMenu { get; set; }
        public bool ShowMenuTop { get; set; }
        public bool ShowMenuHome { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập vị trí xuất hiện danh mục")]
        public int SapxepDanhMuc { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập banner danh mục.")]
        public string BannerDanhMuc { get; set; }
        public string MoTaDanhMuc { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập title SEO")]
        public string SEOtitle { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập Dcription SEO")]
        public string SEODescription { get; set; }
    }
}