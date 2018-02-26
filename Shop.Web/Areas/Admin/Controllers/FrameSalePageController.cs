using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ClosedXML.Excel;
using PagedList;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class FrameSalePageController : Controller
    {

        private readonly IIframeSalepageRepository _iframeSalepageRepository;
        private readonly IMenuRepository _menuRepository;

        public FrameSalePageController(IIframeSalepageRepository iframeSalepageRepository, IMenuRepository menuRepository)
        {
            _iframeSalepageRepository = iframeSalepageRepository;
            _menuRepository = menuRepository;
        }

        private const int pageSize = 20;
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<IframeSalepage> tags = _iframeSalepageRepository.GetAll().OrderByDescending(o => o.NgayTao).ToList();
            var data = tags.ToPagedList(pageNumber, pageSize);
            return View("Index", data);
        }
    }
}
