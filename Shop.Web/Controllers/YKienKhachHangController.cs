using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Repositories;
using Shop.Model;


namespace Shop.Web.Controllers
{
    public class YKienKhachHangController : Controller
    {

        private readonly IYKienKhachHangRepository _kienKhachHangRepository;

        public YKienKhachHangController(IYKienKhachHangRepository kienKhachHangRepository)
        {
            _kienKhachHangRepository = kienKhachHangRepository;
        }

        public ActionResult Index()
        {
            IList<YKienKhachHang> yKienKhachHangs =
                _kienKhachHangRepository.GetAll().OrderByDescending(o => o.NgayTao).Take(8).ToList();
            return View("YKien",yKienKhachHangs);
       }
       
    
    }
}
