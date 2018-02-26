using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class RedirectLinkModel
    {
        public int Redirect301Id { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên bài viết.")]
        public string LinkCu { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập hình đại diện.")]
        public string LinkMoi { get; set; }
        public DateTime NgayTao { get; set; }
    }
}