 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using PagedList;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;

namespace Shop.Admin.Controllers
{
     [ShopAuthorize]
    public class TinTucController : BaseController
    {
        private readonly IDetailMenuRepository _detailMenuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 20;
        public TinTucController(IDetailMenuRepository detailMenuRepository, IUnitOfWork unitOfWork)
        {
            _detailMenuRepository = detailMenuRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<DetailMenu> detail = _detailMenuRepository.GetMany(o => o.id_Menu == 20).OrderByDescending(o => o.sDate).ToList();
            var data = detail.ToPagedList(pageNumber, pageSize);

            return View("Index", data);
        }
        public ActionResult TinBaoChi(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<DetailMenu> detail = _detailMenuRepository.GetMany(o => o.id_Menu == 1462).OrderByDescending(o => o.sDate).ToList();
            var data = detail.ToPagedList(pageNumber, pageSize);
            return View("Tinbaochi", data);
        }
        public ActionResult TinKhuyenMai(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<DetailMenu> detail = _detailMenuRepository.GetMany(o => o.id_Menu == 1399).OrderByDescending(o => o.sDate).ToList();
            var data = detail.ToPagedList(pageNumber, pageSize);
            return View("Tinkhuyenmai", data);
        }
        public ActionResult add()
        {
            return View("Create");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Addnew(ThemTinTucModel model)
        {
            if (ModelState.IsValid)
            {
            DetailMenu tintuc=new DetailMenu()
                                  {
                                      sDate = DateTime.Now,
                                      Name = RejectMarks(model.Name),
                                      Note = model.Note,
                                      Img = (model.Img).Replace("/files/", ""),
                                      Content = model.Content,
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
                                      ShowKhuyenMai = false,
                                      ShowIconNew = false,
                                      TinhTrangSP = false,
                                      SEODescription = model.SEODescription,
                                      SEOtitle = model.SEOtitle

                                  };
        
            _detailMenuRepository.Add(tintuc);
            _unitOfWork.Commit();
            }
              else
              {
                  return View("Create", model);
              }
            return RedirectToAction("Index");
        }
        public ActionResult edit(int id)
        {
            var idcu = _detailMenuRepository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(DetailMenu model)
        {
            DetailMenu dlcu = _detailMenuRepository.GetById(model.id_);
            dlcu.id_ = model.id_;
            dlcu.Name = model.Name;
            dlcu.Img = (model.Img).Replace("/files/", "");
            dlcu.Link = ConvertFont(model.Name);
            dlcu.id_ = model.id_;
            dlcu.SEODescription = model.SEODescription;
            dlcu.SEOtitle = model.SEOtitle;
            _detailMenuRepository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index"); 
        }
        public ActionResult Delete(int id)
        {
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
        public string RejectMarks(string text)
        {
            text = text.ToLower();
            string[] pattern = new string[7];

            pattern[0] = "a|(á|ả|à|ạ|ã|ă|ắ|ẳ|ằ|ặ|ẵ|â|ấ|ẩ|ầ|ậ|ẫ)";

            pattern[1] = "o|(ó|ỏ|ò|ọ|õ|ô|ố|ổ|ồ|ộ|ỗ|ơ|ớ|ở|ờ|ợ|ỡ)";

            pattern[2] = "e|(é|è|ẻ|ẹ|ẽ|ê|ế|ề|ể|ệ|ễ)";

            pattern[3] = "u|(ú|ù|ủ|ụ|ũ|ư|ứ|ừ|ử|ự|ữ)";

            pattern[4] = "i|(í|ì|ỉ|ị|ĩ)";

            pattern[5] = "y|(ý|ỳ|ỷ|ỵ|ỹ)";

            pattern[6] = "d|đ";

            for (int i = 0; i < pattern.Length; i++)
            {
                // kí tự sẽ thay thế

                char replaceChar = pattern[i][0];

                MatchCollection matchs = Regex.Matches(text, pattern[i]);

                foreach (Match m in matchs)
                {
                    text = text.Replace(m.Value[0], replaceChar);
                }
            }
            text = Regex.Replace(text, @"[^\w\.@]", " ").Trim().ToLower();
            text = text.Replace(" ", " ");
            return text;
        }
    }
}
