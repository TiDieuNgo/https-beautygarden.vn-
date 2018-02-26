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
    public class AMPTinTucController : Controller
    {
        private readonly IDetailMenuRepository _detailMenuRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPromotionRepository _promotionRepository;
        private const int pageSize = 5;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReviewRepository _reviewRepository;

        public AMPTinTucController(IDetailMenuRepository detailMenuRepository, IMenuRepository menuRepository, IPromotionRepository promotionRepository, IUnitOfWork unitOfWork, IReviewRepository reviewRepository)
        {
            _detailMenuRepository = detailMenuRepository;
            _menuRepository = menuRepository;
            _promotionRepository = promotionRepository;
            _unitOfWork = unitOfWork;
            _reviewRepository = reviewRepository;
        }

        public ActionResult Index(string linkttamp)
        {
            // int id = _detailMenuRepository.GetIdTinTuc(linkttamp);
            //tu id cua san pham lay duoc no thuoc danh muc tin tuc nao
            DetailMenu tinTuc = _detailMenuRepository.Get(o => o.Link.Equals(linkttamp));
            if (tinTuc.TinhTrangSP == true && tinTuc.ShowKhuyenMai == false)//su kien khuyen mai
            {
                ViewData["duongdan"] = "/su-kien-khuyen-mai.html";
                ViewData["ten"] = "Sự Kiện Khuyến mãi";

            }
            else
            {
                ViewData["duongdan"] = "/bi-quyet-lam-dep.html";
                ViewData["ten"] = "Bí Quyết Làm Đẹp";
            }

            TinTucModel model = new TinTucModel()
            {
                //tin thuoc danh muc dang xem va khac id dang xem
                tinkhacs = _detailMenuRepository.GetMany(o => o.id_ != tinTuc.id_ && !string.IsNullOrEmpty(o.Link)).Take(10).OrderByDescending(o => o.sDate).ToList(),
                TinTucChiTiet = tinTuc
            };
            _detailMenuRepository.Update(tinTuc);
            _unitOfWork.Commit();
            return View("ChiTiet", model);
        }
    
    }
}
