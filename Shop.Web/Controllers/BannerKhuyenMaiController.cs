using System;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Model;

namespace Shop.Web.Controllers
{
     [WhitespaceFilter]
    public class BannerKhuyenMaiController : BaseController
     {
         private readonly IBannerKhuyenMaiRepository _bannerKhuyenMaiRepository;
         private readonly IUnitOfWork _unitOfWork;

         public BannerKhuyenMaiController(IBannerKhuyenMaiRepository bannerKhuyenMaiRepository, IUnitOfWork unitOfWork)
         {
             _bannerKhuyenMaiRepository = bannerKhuyenMaiRepository;
             _unitOfWork = unitOfWork;
         }

        public ActionResult Index()
        {
            BannerKhuyenMaiModel model =new BannerKhuyenMaiModel()
                                        {
                                            BannerKhuyenMais = _bannerKhuyenMaiRepository.GetAll().OrderByDescending(o=>o.NgayTao).ToList(),
                                            Banner = _bannerKhuyenMaiRepository.GetAll().FirstOrDefault()
                                        };
            return View("Index", model);
        }
       
    }
}
