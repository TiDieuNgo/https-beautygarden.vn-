using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class ThemAccountModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên đăng nhập")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập mật khẩu")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập họ và tên")]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập Email.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Nhập Sai")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Email Nhập Sai")]
        public string Email { get; set; }
        public int Permission { get; set; }
        public bool Show { get; set; }
    }
}