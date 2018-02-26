using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
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
    public class RedirectLinkController : BaseController
    {
        private readonly IRedirect301Repository _redirect301Repository;
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int pageSize = 50;
        private readonly HttpContextBase _httpContext;
        public RedirectLinkController(IRedirect301Repository redirect301Repository, IMenuRepository menuRepository, IUnitOfWork unitOfWork, HttpContextBase httpContext)
        {
            _redirect301Repository = redirect301Repository;
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }
        public ActionResult Index(int? page)
        {
            //su kien khuyen mai
            int pageNumber = (page ?? 1);
            IList<Redirect301> detail = _redirect301Repository.GetAll().OrderByDescending(o => o.NgayTao).ToList();
            var data = detail.ToPagedList(pageNumber, pageSize);
            return View("Index", data);
        }
        public ActionResult add()
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.redirect_create)))
                return View("_NoAuthor");
            return View("Create");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Addnew(RedirectLinkModel model)
        {
            if (ModelState.IsValid)
            {
                Redirect301 tintuc = new Redirect301()
                {
                    LinkCu = model.LinkCu,
                    LinkMoi = model.LinkMoi,
                    NgayTao = DateTime.Now
                };

                _redirect301Repository.Add(tintuc);
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
            if (!CheckRole(_httpContext, int.Parse(Roles.redirect_Edit)))
                return View("_NoAuthor");

            var idcu = _redirect301Repository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(Redirect301 model)
        {
            Redirect301 dlcu = _redirect301Repository.GetById(model.Redirect301Id);
            dlcu.LinkCu = model.LinkCu;
            dlcu.LinkMoi = model.LinkMoi;
            _redirect301Repository.Update(dlcu);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var idxoa = _redirect301Repository.GetById(id);
            _redirect301Repository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
          
    }
}
