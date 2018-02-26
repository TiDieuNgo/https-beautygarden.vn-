using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Repositories;
using Shop.Model;

namespace Shop.Web.Controllers
{
    public class HeThongCuaHangController : Controller
    {
        private readonly IHeThongCuaHangRepository _heThongCuaHangRepository;

        public HeThongCuaHangController(IHeThongCuaHangRepository heThongCuaHangRepository)
        {
            _heThongCuaHangRepository = heThongCuaHangRepository;
        }

        public ActionResult Index()
        {
            IList<HeThongCuaHang> model = _heThongCuaHangRepository.GetAll().OrderBy(o => o.SapXep).ToList();
            return View("HeThongShowRoom", model);
        }
    }
}
