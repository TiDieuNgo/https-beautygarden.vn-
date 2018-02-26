using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class ReviewModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên bài viết.")]
        public string TieuDe { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập hình đại diện.")]
        public string HinhAnh { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập mô tả cho bài viết.")]
        public string Mota { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập link cho bài viết.")]
        public string Link { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập vị trí.")]
        public int Sapxep { get; set; }
        public string ChiTiet { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập title SEO")]
        public string SEOtitle { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập Description SEO")]
        public string SEODescription { get; set; }
        //  lưu tag
        public string mySingleField { get; set; }
    }
}