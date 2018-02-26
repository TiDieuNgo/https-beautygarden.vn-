using System.Linq;
using System.Web.Mvc;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Model;


namespace Shop.Web.Controllers
{
    public class NoiDungFooterController : BaseController
    {
        private readonly IMenuRepository _menuRepository;
        public NoiDungFooterController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public ActionResult ChiTiet(string linkfooter)
        {
            int id = _menuRepository.GetIdMenuFromLinkFooter(linkfooter);
            Menu noidung = _menuRepository.GetById(id);
            NoiDungFooterModel model = new NoiDungFooterModel()
            {
                tinkhacs = _menuRepository.GetMany(o => o.id_ != id).Take(10).OrderByDescending(o => o.sDate).ToList(),
                TinTucChiTiet = noidung
            };
            return View("ChiTiet", model);
        }
    }
}
