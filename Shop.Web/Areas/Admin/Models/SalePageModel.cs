using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Web.Areas.Admin.Models
{
    public class SalePageModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tiêu đề.")]
        public string TieuDe { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập số điện thoại.")]
        public string SDT { get; set; }
        public string NoiDung1 { get; set; }
        public string NoiDung2 { get; set; }
        public string NoiDung3 { get; set; }
        public string NoiDung4 { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập SeoTitle.")]
        public string SeoTitle { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập SeoDecription.")]
        public string SeoDecription { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập KeyWord.")]
        public string KeyWord { get; set; }
        public DateTime NgayTao { get; set; }
        public string Link { get; set; }
        public string CodeName { get; set; }
        public string HinhAnh { get; set; }
    }
}