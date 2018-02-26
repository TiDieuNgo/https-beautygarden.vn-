using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Shop.Data;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Models;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class ReviewController : BaseController
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpContextBase _httpContext;
        private readonly ITagTinTucRepository _tagTinTucRepository;
        public ReviewController(IReviewRepository reviewRepository, IUnitOfWork unitOfWork, HttpContextBase httpContext, ITagTinTucRepository tagTinTucRepository)
        {
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
            _tagTinTucRepository = tagTinTucRepository;
        }

        private const int pageSize = 20;


        public ActionResult Index(int? page)
        {
            //review
            int pageNumber = (page ?? 1);
            IList<Review> detail = _reviewRepository.GetAll().OrderByDescending(o => o.Sdate).ToList();
            var data = detail.ToPagedList(pageNumber, pageSize);

            return View("Index", data);
        }

        public ActionResult add()
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Tintuc_Create)))
                return View("_NoAuthor");
            return View("Create");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Addnew(ReviewModel model)
        {
            //if (ModelState.IsValid)
            //{
                Review tintuc = new Review()
                {
                    Sdate = DateTime.Now,
                    TieuDe = RejectMarks(model.TieuDe),
                    Mota = model.Mota,
                    HinhAnh = (model.HinhAnh).Replace("/files/", ""),
                    ChiTiet = ChangeImageSEO(model.ChiTiet, model.TieuDe, ConvertFont(model.TieuDe)),
                    //ChiTiet = model.ChiTiet,
                    Ok = false,
                    Sapxep = model.Sapxep,
                    Link = ConvertFont(model.TieuDe),
                    SEODescription = model.SEODescription,
                    SEOtitle = model.SEOtitle,
                    NguoiTao = User.Identity.Name
                };

                _reviewRepository.Add(tintuc);
                _unitOfWork.Commit();
                if (model.mySingleField != null)
                {
                    #region thêm mới tag

                    string[] separators = { "," };
                    var taglist = model.mySingleField.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    //check null: nếu tồn tại thêm mới bình thường
                    if (taglist.Any())
                    {
                        foreach (var tag in taglist)
                        {
                            TagTinTuc _tag = new TagTinTuc()
                                             {
                                                 NgayTao = DateTime.Now,
                                                 IdMenu = tintuc.id_, // id cua tin moi vua them
                                                 TenTag = tag.Trim(),
                                                 Link = ConvertFont(tag.Trim()),
                                                 Code = RejectMarks(tag.Trim())

                                             };
                            _tagTinTucRepository.Add(_tag);
                            _unitOfWork.Commit();
                        }
                    }
                    #endregion
                }
            //}
            //else
            //{
            //    return View("Create", model);
            //}
            return RedirectToAction("Index");
        }
        public ActionResult edit(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Tintuc_Edit)))
                return View("_NoAuthor");
            var idcu = _reviewRepository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(ReviewModel model)
        {
            Review dlcu = _reviewRepository.GetById(model.id_);
            dlcu.id_ = model.id_;
            dlcu.TieuDe = model.TieuDe;
            dlcu.HinhAnh = (model.HinhAnh).Replace("/files/", "");
            dlcu.Link = model.Link;
            dlcu.id_ = model.id_;
            dlcu.Mota = model.Mota;
            dlcu.Sapxep = model.Sapxep;
            dlcu.Sdate = DateTime.Now;
            //dlcu.ChiTiet = ChangeImageSEO(model.ChiTiet, model.TieuDe, ConvertFont(model.TieuDe));
            dlcu.ChiTiet = model.ChiTiet;
            dlcu.SEODescription = model.SEODescription;
            dlcu.SEOtitle = model.SEOtitle;
            dlcu.NguoiTao = User.Identity.Name;
            _reviewRepository.Update(dlcu);
            _unitOfWork.Commit();
            if (model.mySingleField != null)
            {
                #region danh sach tag
                string[] separators = { "," };
                var taglist = model.mySingleField.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                //check null: nếu tồn tại thêm mới bình thường
                if (taglist.Any())
                {
                    foreach (var tag in taglist)
                    {
                        //check trung
                        int checktrung = _tagTinTucRepository.GetTagNamebyIdmenu(model.id_, tag.Trim());
                        if (checktrung == 0)
                        {
                            TagTinTuc _tag = new TagTinTuc()
                                             {
                                                 NgayTao = DateTime.Now,
                                                 IdMenu = dlcu.id_,
                                                 TenTag = tag.Trim(),
                                                 Link = ConvertFont(tag.Trim()),
                                                 Code = RejectMarks(tag.Trim())
                                             };
                            _tagTinTucRepository.Add(_tag);
                            _unitOfWork.Commit();
                        }
                    }
                }
                #endregion
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Tintuc_Delete)))
                return View("_NoAuthor");

            var idxoa = _reviewRepository.GetById(id);
            _reviewRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult UpdateOk(int idtintuc, bool Checked)
        {
            if (Checked)
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Reviews set Ok='True' where id_ ={0}", idtintuc);

                }
            }
            else
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update Reviews set Ok='False' where id_ ={0}", idtintuc);

                }
            }

            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }


    }
}
