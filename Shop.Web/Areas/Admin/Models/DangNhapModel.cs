using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Areas.Admin.Models
{
    public class DangNhapModel
    {
        [Required(ErrorMessage = "Bạn Chưa Nhập Tên Đăng Nhập.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bạn Vui Lòng Nhập Mật Khẩu.")]
        public string Password { get; set; }
    }
}
