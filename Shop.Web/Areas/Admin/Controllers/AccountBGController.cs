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
using Shop.Web.Core.Common;
using Shop.Web.Core.Models;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class AccountBGController : BaseController
    {
        private readonly IAccount298Repository _account298Repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpContextBase _httpContext;
        private const int pageSize = 20;
        public AccountBGController(IAccount298Repository account298Repository, IUnitOfWork unitOfWork, HttpContextBase httpContext)
        {
            _account298Repository = account298Repository;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            IList<Account298> tags = _account298Repository.GetAll().OrderByDescending(o => o.Username).ToList();
            var data = tags.ToPagedList(pageNumber, pageSize);
            return View("Index", data);

        }
        public ActionResult add()
        {
            //khong co quyen thi khong tao user được
            if (!CheckRole(_httpContext, int.Parse(Roles.User_Create)))
                return View("_NoAuthor");

            return View("Create");

        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNew(ThemAccountModel model)
        {
            if (ModelState.IsValid)
            {
                Account298 ac = new Account298()
                                  {
                                      Email = model.Email,
                                      Username = model.Username,
                                      Fullname = model.Fullname,
                                      Show = model.Show,
                                      Permission = model.Permission,
                                      Password = Md5Encrypt.Md5EncryptPassword(model.Password),
                                  };
                _account298Repository.Add(ac);
                _unitOfWork.Commit();
            }
            else
            {
                return View("Create", model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.User_Edit)))
                return View("_NoAuthor");

            var idcu = _account298Repository.GetById(id);
            return View("Edit", idcu);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(Account298 model)
        {
            Account298 ac = _account298Repository.GetById(model.id_);
            ac.id_ = model.id_;
            ac.Email = model.Email;
            ac.Fullname = model.Fullname;
            ac.Password = Md5Encrypt.Md5EncryptPassword(model.Password);
            ac.Permission = model.Permission;
            ac.Show = model.Show;
            ac.Username = model.Username;
            _account298Repository.Update(ac);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult delete(int id)
        {
            //khong co quyen khong xoa duoc
            if (!CheckRole(_httpContext, int.Parse(Roles.User_Delete)))
                return View("_NoAuthor");

            var idxoa = _account298Repository.GetById(id);
            _account298Repository.Delete(idxoa);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult DoiMatKhau(Account298 model)
        {
            //load session dang luu roi doi mat khau cua thang do luon
            return View("Doimatkhau");
        }
    }
}
