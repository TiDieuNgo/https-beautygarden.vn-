using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Model;


namespace Shop.Web.Controllers
{
    public class HoiDapSanPhamController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITraLoiCommentRepository _traLoiCommentRepository;
        public HoiDapSanPhamController(ICommentRepository commentRepository, IUnitOfWork unitOfWork, IMenuRepository menuRepository, ITraLoiCommentRepository traLoiCommentRepository)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _menuRepository = menuRepository;
            _traLoiCommentRepository = traLoiCommentRepository;
        }
        public ActionResult Index(int idsanpham)
        {
            CommentModel model = new CommentModel()
            {
                Comments = _commentRepository.GetMany(o => o.IdSanPham == idsanpham && o.Ok).OrderByDescending(o => o.NgayCmt).ToList()
            };
            return View("Index", model);
        }
        public ActionResult TraLoiCauHoi(int id)
        {
            Comment comment = _commentRepository.GetById(id);
            var idsanpham = _menuRepository.GetIdspByCommentId(id);
            ViewData["tenSP"] = _menuRepository.GetTenSanPham(idsanpham);
            TraLoiCommentModel model = new TraLoiCommentModel()
            {
                Comment = comment,
                CauHoiKhacs = _commentRepository.GetMany(o => o.CommentId != id && o.IdSanPham == idsanpham).ToList(),
                TraLoiComments = _traLoiCommentRepository.GetMany(o => o.CommentId == id).ToList()
            };
            return View("TraLoiCauHoi", model);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult AddComment(int idsanPham, string noidung_cmt)
        {
            Comment comment = new Comment()
            {
                IdSanPham = idsanPham,
                NgayCmt = DateTime.Now,
                NoiDungCmt = noidung_cmt
            };
            _commentRepository.Add(comment);
            _unitOfWork.Commit();
            return Json(new
            {
                message = "Thêm mới thành công!"
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult SaveTraLoiComment(int comemntid, string noidung_cmt)
        {
            TraLoiComment model = new TraLoiComment()
            {
                CommentId = comemntid,
                NgayCmt = DateTime.Now,
                NoiDungCmt = noidung_cmt,
            };
            _traLoiCommentRepository.Add(model);
            _unitOfWork.Commit();
            return Json(new
            {
                message = "Thêm mới thành công!"
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TraLoiCauHoiShowHome(int idcomment)
        {
            IList<TraLoiComment> traLoiComment =
                 _traLoiCommentRepository.GetMany(o => o.CommentId == idcomment).OrderByDescending(o => o.NgayCmt).
                     ToList();
            return View("TraLoiCauHoiShowHome", traLoiComment);
        }

        public ActionResult Allcauhoiocautraloi(int id)
        {
            //lay tat ca cau hoi co cau tra loi theo id san pham
            ViewData["tenSP"] = _menuRepository.GetTenSanPham(id);
            CommentModel model = new CommentModel()
            {
                Comments = _commentRepository.GetMany(o => o.IdSanPham == id && o.Ok).OrderByDescending(o => o.NgayCmt).ToList()
            };
            return View("AllCauHoiCoTraLoi", model);
        }
    }
}
