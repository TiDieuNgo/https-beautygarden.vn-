using ActionMailer.Net.Mvc;
using Shop.Web.Models;
using Shop.Web.ViewModels;

namespace Shop.Web.Controllers.Mailers
{
    public class MailersController : MailerBase
    {
        public EmailResult GoiMailLienHe(LienHeModel model)
        {
            To.Add(model.HuongDanSuDung);
            CC.Add("dongthanhsonit@gmail.com");
            From = "dongthanhsonit@gmail.com";
            Subject = "Thông tin liên hệ Beautygarden";
            return Email("GoiMailLienHe", model);
        }
        public EmailResult GoiMailThanhToan(CartViewModel.OrderModel model)
        {
            To.Add(model.CartForm.Email);
            CC.Add("dongthanhsonit@gmail.com");
            From = "dongthanhsonit@gmail.com";
            Subject = "Đơn đặt hàng tại Beautygarden";
            return Email("GoiMailThanhToan", model);
        }
        public EmailResult GoiMailDangKy(KhachHangModel model)
        {
            To.Add(model.Email);
            CC.Add("dongthanhsonit@gmail.com");
            From = "dongthanhsonit@gmail.com";
            Subject = "Thông tin đăng ký thành viên tại Beautygarden!";
            return Email("GoiMailDangKy", model);
        }
        
    }
}
