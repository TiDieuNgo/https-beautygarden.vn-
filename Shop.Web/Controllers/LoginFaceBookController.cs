using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Facebook;

namespace Shop.Web.Controllers
{
    public class LoginFaceBookController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccMember298Repository _accMember298Repository;

        public LoginFaceBookController(IUnitOfWork unitOfWork, IAccMember298Repository accMember298Repository)
        {
            _unitOfWork = unitOfWork;
            _accMember298Repository = accMember298Repository;
        }
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        [AllowAnonymous]
        public ActionResult login()
        {
            return View();
        }

        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            return View("Login");
        }
        [AllowAnonymous]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });


            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string firstname = me.first_name;
                string lastname = me.last_name;

                AccMember298 user = new AccMember298()
                {
                    Email = email,
                    sDate = DateTime.Now,
                    Fullname = firstname + " " + lastname,
                    Username = firstname + " " + lastname,
                    sDateOk = DateTime.Now,
                    idUser = 15,
                    idUserOk = 15,
                    Password = "e10adc3949ba59abbe56e057f20f883e",
                    Birdthday = DateTime.Now,
                    Address = "",
                    States = "",
                    Level = 0,
                    Show = true,
                    DiaChiGiaoHang = "",
                    TenCongTy = "",
                    MaSoThue = "",
                    DiaChiCongTy = "",
                    Sex = "Nữ",
                    Phone = "0123456789"
                };
                AccMember298 khachHang = _accMember298Repository.Get(o => o.Email.Equals(user.Email));
                if (khachHang != null)
                {
                    //neu mail co roi chi viec lay len roi xin chao
                    SetCookieLogin(this.Request.RequestContext, user.Email);
                }
                else
                {
                    //neu mail chua co thi luu cai khach hang do vao db
                    SetCookieLogin(this.Request.RequestContext, user.Email);
                    _accMember298Repository.Add(user);
                    _unitOfWork.Commit();
                }
            }
            // Set the auth cookie
            //FormsAuthentication.SetAuthCookie(email, false);
            return RedirectToAction("Index", "Home");
        }
    }
}
