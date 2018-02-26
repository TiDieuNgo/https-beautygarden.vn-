using System.Linq;
using System.Web.Mvc;
using PagedList;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Model;


namespace Shop.Web.Controllers
{
    [WhitespaceFilter]
    public class ReviewController : Controller
    {
        private const int pageSize = 5;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReviewRepository _reviewRepository;
        private readonly ITagTinTucRepository _tagTinTucRepository;

        public ReviewController(IUnitOfWork unitOfWork, IReviewRepository reviewRepository, ITagTinTucRepository tagTinTucRepository)
        {
            _unitOfWork = unitOfWork;
            _reviewRepository = reviewRepository;
            _tagTinTucRepository = tagTinTucRepository;
        }
        public ActionResult Index(int page = 1)
        {
            ReviewModel model = new ReviewModel()
            {
                Reviews = _reviewRepository.GetMany(o => o.Ok).OrderByDescending(o => o.Sdate).ToPagedList(page, pageSize)
            };
            return View("Review", model);
        }

        public ActionResult Chitiet(string seolink)
        {
            Review tinTuc = _reviewRepository.Get(o => o.Link.Equals(seolink));
            ReviewModel model = new ReviewModel()
            {
                Reviewkhacs = _reviewRepository.GetMany(o => o.id_ != tinTuc.id_ && !string.IsNullOrEmpty(o.Link)).Take(10).OrderByDescending(o => o.Sdate).ToList(),
                Review = tinTuc,
                TagTinTucs = _tagTinTucRepository.GetTenTagForReview(seolink)
            };
            _reviewRepository.Update(tinTuc);
            _unitOfWork.Commit();
            return View("ChiTiet", model);
        }
    }
}
