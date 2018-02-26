using System.Linq;
using System.Web.Mvc;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class CommentController : BaseController
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CommentController(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var dl =
                _commentRepository.GetAll().OrderByDescending(o => o.NgayCmt);
            return View(dl);
        }

        [HttpPost]
        public JsonResult UpdateOk(int idsanpham, bool Checked)
        {
            if (Checked)
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Comments set Ok='True' where CommentId ={0}", idsanpham);
                }
            }
            else
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Comments set Ok='False' where CommentId ={0}", idsanpham);
                }
            }
            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }

        public ActionResult Delete(int id)
        {
            var idxoa = _commentRepository.GetById(id);
            _commentRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
