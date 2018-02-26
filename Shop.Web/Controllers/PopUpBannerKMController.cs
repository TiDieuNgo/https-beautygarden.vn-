using System;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;

namespace Shop.Web.Controllers
{
    [WhitespaceFilter]
    public class PopUpBannerKMController : BaseController
    {
        private readonly IPopUpBannerKMRepository _popUpBannerKmRepository;

        public PopUpBannerKMController(IPopUpBannerKMRepository popUpBannerKmRepository)
        {
            _popUpBannerKmRepository = popUpBannerKmRepository;
        }

        public ActionResult Index()
        {
            PopUpBannerKM bannerQCtrangchu = _popUpBannerKmRepository.GetAll().FirstOrDefault();
            return View("Index", bannerQCtrangchu);
        }
    }
}
