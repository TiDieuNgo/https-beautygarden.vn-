using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Models
{
    public class KhachHangModel
    {
        public int id_ { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập Email.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email nhập sai")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Email nhập sai")]
        public string Email { get; set; }
        [StringLength(100, ErrorMessage = "{0} phải lớn hơn {2} kí tự.", MinimumLength = 6)]
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Bạn vui lòng nhập mật khẩu.")]
        public string Password { get; set; }
    }
}