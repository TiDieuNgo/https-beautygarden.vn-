using System.ComponentModel.DataAnnotations;

namespace Shop.Web.Model
{
    public class DanhNhapModel
    {
        [DataType(DataType.EmailAddress, ErrorMessage = "Email nhập sai")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Email nhập sai")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "{0} phải lớn hơn {2} kí tự.", MinimumLength = 6)]
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Bạn vui lòng nhập Mật Khẩu.")]
        public string Password { get; set; }
    }
}