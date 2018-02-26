using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Authentication;
using Shop.Web.Core.Extensions;
using Shop.Web.Core.Models;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class PhanQuyenController : BaseController
    {
        private readonly IAccount298Repository _account298Repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFormsAuthentication formAuthentication;
        private readonly HttpContextBase _httpContext;

        public PhanQuyenController(IAccount298Repository account298Repository, IUnitOfWork unitOfWork, IFormsAuthentication formAuthentication, HttpContextBase httpContext)
        {
            _account298Repository = account298Repository;
            _unitOfWork = unitOfWork;
            this.formAuthentication = formAuthentication;
            _httpContext = httpContext;
        }


        public ActionResult Index()
        {
            //co quyen moi duoc xem
            if (!CheckRole(_httpContext, int.Parse(Roles.Phan_Quyen_View)))
                return View("_NoAuthor");

            ShopUser userLogin = _httpContext.User.GetShopUser();
            IList<Account298> account298s = _account298Repository.GetAll().ToList();
            IList<PhanQuyenModel.QuyenModel> models = new List<PhanQuyenModel.QuyenModel>();
            IList<RoleList> roles = Roles.GetRoles();

            if (account298s.Any())
            {
                foreach (var account298 in account298s)
                {
                    //cat tu chuoi sang list int
                    IList<int> ids = !string.IsNullOrEmpty(account298.Roles)
                                         ? account298.Roles.Split(',').Select(o => Convert.ToInt32(o)).ToList()
                                         : new List<int>();

                    IList<RoleList> QuyenUsers = (from roleList in roles
                                                  where ids.Contains(roleList.Id)
                                                  select roleList).ToList();

                    IList<string> tens = QuyenUsers.Select(o => o.Name).ToList();
                    models.Add(new PhanQuyenModel.QuyenModel()
                    {
                        NhanVienId = account298.id_,
                        TenNhanVien = account298.Fullname,
                        Quyen = string.Join(",", tens)
                    });

                }
            }
            return View(models);
        }
        public ActionResult Update(int id)
        {
            //co quyen moi duoc update
            if (!CheckRole(_httpContext, int.Parse(Roles.Phan_Quyen_Update)))
                return View("_NoAuthor");

            Account298 nhanVien = _account298Repository.GetById(id);
            IList<int> ids = !string.IsNullOrEmpty(nhanVien.Roles)
                                        ? nhanVien.Roles.Split(',').Select(o => Convert.ToInt32(o)).ToList()
                                        : new List<int>();
            IList<RoleList> roles = Roles.GetRoles();

            IList<PhanQuyenModel.SetQuyen> phanQuyenModels = new List<PhanQuyenModel.SetQuyen>();

            ViewData["TenNhanVien"] = nhanVien.Fullname;
            foreach (var r in roles)
            {
                phanQuyenModels.Add(new PhanQuyenModel.SetQuyen()
                {
                    NhanVienId = id,
                    IdQuyen = r.Id,
                    Selected = ids.Contains(r.Id),
                    TenQuyen = r.Name

                });

            }
            return View(phanQuyenModels);
        }
        [HttpPost]
        public JsonResult save(PhanQuyenModel.SaveModel form)
        {
            Account298 nhanVien = _account298Repository.GetById(form.idNhanVien);
            IList<int> ids = !string.IsNullOrEmpty(nhanVien.Roles)
                                       ? nhanVien.Roles.Split(',').Select(o => Convert.ToInt32(o)).ToList()
                                       : new List<int>();
            if (form.Checked)
            {
                if (!ids.Any(o => o == form.IdQuyen))
                    ids.Add(form.IdQuyen);
            }
            else
            {
                ids.Remove(form.IdQuyen);
            }
            nhanVien.Roles = string.Join(",", ids);
            _account298Repository.Update(nhanVien);
            _unitOfWork.Commit();

            formAuthentication.SetAuthCookie(this.HttpContext,
                                                       UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                                           nhanVien));

            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult saveAll(PhanQuyenModel.SaveModel form)
        {
            Account298 nhanVien = _account298Repository.GetById(form.idNhanVien);
            IList<RoleList> roles = Roles.GetRoles();

            IList<int> ids = new List<int>();
            if (form.Checked)
            {
                ids = roles.Select(o => o.Id).ToList();
            }

            nhanVien.Roles = string.Join(",", ids);
            _account298Repository.Update(nhanVien);
            _unitOfWork.Commit();

            formAuthentication.SetAuthCookie(this.HttpContext,
                                                         UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                                             nhanVien));
            return Json(new { ok = true, JsonRequestBehavior.AllowGet });
        }
    }
}
