using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
        [ShopAuthorize]
    public class DonHangController : Controller
    {
        private readonly IDetailMenuCommentRepository _detailMenuCommentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;
        public DonHangController(IDetailMenuCommentRepository detailMenuCommentRepository, IUnitOfWork unitOfWork)
        {
            _detailMenuCommentRepository = detailMenuCommentRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<DetailMenuComment> tags = _detailMenuCommentRepository.GetAll().OrderByDescending(o => o.sDate).ToList();
            var data = tags.ToPagedList(pageNumber, pageSize);
            return View("Index", data);
           
        }
        public ActionResult ThongKe()
        {
            IList<DetailMenuComment> tags = _detailMenuCommentRepository.GetAll().OrderByDescending(o => o.sDate).ToList();
            ViewData["thongkedonhang"] = tags.Count();
            return View("ThongKe");
        }
        public ActionResult ChiTietdonhang(int id)
        {
            DetailMenuComment donHang = _detailMenuCommentRepository.GetById(id);
            IList<DetailMenuCommentItem> chiTietDonHangs = donHang.ChiTietDonHangs.ToList();

            OrderModel model = new OrderModel()
            {
                DonHang = donHang,
                ChiTietDonHangs = chiTietDonHangs

            };

            return View(model);
        }
       public ActionResult delete(int id)
       {
           var idxoa = _detailMenuCommentRepository.GetById(id);
           _detailMenuCommentRepository.Delete(idxoa);
           _unitOfWork.Commit();
           return RedirectToAction("Index");
       }
    }
}
