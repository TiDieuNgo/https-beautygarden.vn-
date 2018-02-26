using System.ComponentModel.DataAnnotations;
using System.Web;
using Newtonsoft.Json;
using Shop.Web.Core.Helper;

namespace Shop.Web.Models
{
    public class LienHeModel
    {
        public int id_ { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập tên.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập địa chỉ.")]
        public string GiaoHang { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập Email.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Nhập Sai")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Email Nhập Sai")]
        public string HuongDanSuDung { get; set; }
        [Required(ErrorMessage = "Bạn vui lòng nhập điện thoại.")]
        public string Link { get; set; }
        public string Content { get; set; }
        public LienHeModel()
        {
            CookieHelper cookieHelper = new CookieHelper("User_Infor");

            if (cookieHelper.GetCookie() != null)
            {
                string json = HttpUtility.UrlDecode(cookieHelper.GetCookie().Values["User_Infor"]);
                if (!string.IsNullOrEmpty(json))
                {
                    Shop.Web.Models.KhachHangModel form = JsonConvert.DeserializeObject<Shop.Web.Models.KhachHangModel>(json);
                    Link = form.Phone;
                    HuongDanSuDung = form.Email;
                }

            }
        }
    }
}