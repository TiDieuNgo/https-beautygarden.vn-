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
    public class BiQuyetLamDepController : BaseController
    {
        private readonly IDetailMenuRepository _detailMenuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;
        private readonly HttpContextBase _httpContext;
        private readonly ITagTinTucRepository _tagTinTucRepository;
        private readonly ISalePageRepository _salePageRepository;
        public BiQuyetLamDepController(IDetailMenuRepository detailMenuRepository, IUnitOfWork unitOfWork, HttpContextBase httpContext, ITagTinTucRepository tagTinTucRepository, ISalePageRepository salePageRepository)
        {
            _detailMenuRepository = detailMenuRepository;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
            _tagTinTucRepository = tagTinTucRepository;
            _salePageRepository = salePageRepository;
        }

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<DetailMenu> detail = _detailMenuRepository.GetMany(o => o.id_Menu == 20 && o.ShowMenu == true && o.TinhTrangSP == false && o.ShowKhuyenMai == true).OrderByDescending(o => o.sDate).ToList();
            var data = detail.ToPagedList(pageNumber, pageSize);

            return View("Index", data);
        }

        public ActionResult add()
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Tintuc_Create)))
                return View("_NoAuthor");
            ViewBag.SalePages = _salePageRepository.GetAll().OrderByDescending(o => o.NgayTao).ToList();
            return View("Create");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Addnew(ThemTinTucModel model)
        {
            //if (ModelState.IsValid)
            //{
                #region them moi tin tuc

                DetailMenu tintuc = new DetailMenu()
                {
                    sDate = DateTime.Now,
                    Name = RejectMarks(model.Name),
                    Note = model.Note,
                    Img = (model.Img).Replace("/files/", ""),
                    Content = ChangeImageSEO(model.Content, model.Name, ConvertFont(model.Name)),
                    //Content = model.Content,
                    id_Menu = 20,
                    Link = ConvertFont(model.Name),
                    idUser = 15,
                    idUserOk = 15,
                    sDateOk = DateTime.Now,
                    ok = true,
                    Ma_Hang = "",
                    NameAdwords = "",
                    Number = 0,
                    ShowMenu = true,
                    ShowIconHot = false,
                    ShowKhuyenMai = true,
                    ShowIconNew = false,
                    TinhTrangSP = false,
                    sPosition = model.sPosition,
                    SEODescription = model.SEODescription,
                    SEOtitle = model.SEOtitle,
                    CodeName = ConvertFont(model.Name).Replace("-", " "),
                    IdSalePage = model.IdSalePage,
                    NguoiTao = User.Identity.Name
                };

                _detailMenuRepository.Add(tintuc);
                _unitOfWork.Commit();

            #endregion

            if (model.mySingleField != null)
            {
                #region them moi tag

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
            var idcu = _detailMenuRepository.GetById(id);
            ViewBag.SalePages = _salePageRepository.GetAll().OrderByDescending(o => o.NgayTao).ToList();
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(ThemTinTucModel model)
        {
            #region update tin tuc

            DetailMenu dlcu = _detailMenuRepository.GetById(model.id_);
            dlcu.id_ = model.id_;
            dlcu.Name = model.Name;
            dlcu.Img = (model.Img).Replace("/files/", "");
            dlcu.Link = model.Link;
            dlcu.id_ = model.id_;
            dlcu.Note = model.Note;
            dlcu.sPosition = model.sPosition;
            dlcu.sDate = DateTime.Now;
            dlcu.sDateOk = DateTime.Now;
            //dlcu.Content =  ChangeImageSEO(model.Content, model.Name, ConvertFont(model.Name));
            dlcu.Content = model.Content;
            dlcu.SEODescription = model.SEODescription;
            dlcu.SEOtitle = model.SEOtitle;
            dlcu.CodeName = ConvertFont(model.Name).Replace("-", " ");
            dlcu.IdSalePage = model.IdSalePage;
            dlcu.NguoiTao = User.Identity.Name;
            _detailMenuRepository.Update(dlcu);
            _unitOfWork.Commit();

            #endregion

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

            var idxoa = _detailMenuRepository.GetById(id);
            _detailMenuRepository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        //tim kiem san pham
        public ActionResult TimKiem(string keyword, int? page)
        {
            int pageNumber = (page ?? 1);
            keyword = RejectMarks(keyword);

            IList<DetailMenu> sanPhams = _detailMenuRepository.TimKiem(keyword.Trim().ToLower());

            TinTucModel timkiem = new TinTucModel()
            {
                DetailMenus = sanPhams.ToPagedList(pageNumber, pageSize),

                soluongtimduoc = sanPhams.Count(),
            };
            ViewData["key"] = keyword;
            return View(timkiem);
        }
        [HttpPost]
        public JsonResult UpdateOk(int idtintuc, bool Checked)
        {
            if (Checked)
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update DetailMenu set ok='True' where id_ ={0}", idtintuc);

                }
            }
            else
            {
                using (var context = new ShopDataContex())
                {
                    context.Database.ExecuteSqlCommand("update DetailMenu set ok='False' where id_ ={0}", idtintuc);

                }
            }

            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }

        //  load tag khi edit
        public JsonResult setValEdit(int IdMenu)
        {
            IList<TagName> _tagedit = _detailMenuRepository.GetTagEdit(IdMenu);
            return Json(_tagedit, JsonRequestBehavior.AllowGet);
        }
    }
}
