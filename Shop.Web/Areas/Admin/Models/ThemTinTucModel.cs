using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class ThemTinTucModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên bài viết.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập hình đại diện.")]
        public string Img { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập Link bài viết.")]
        public string Link { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập mô tả cho bài viết.")]
        public string Note { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập vị trí.")]
        public int sPosition { get; set; }
        public string Content { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập title SEO")]
        public string SEOtitle { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập Description SEO")]
        public string SEODescription { get; set; }
        //  lưu tag
        public string mySingleField { get; set; }
        public int IdSalePage { get; set; }
    }
}