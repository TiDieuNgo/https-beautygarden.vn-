
using System;
using System.Web;
using System.Web.Security;
using Shop.Web.Core.Models;
using Shop.Model;


namespace Shop.Web.Core.Authentication
{
    public class UserAuthenticationTicketBuilder
    {
        public static FormsAuthenticationTicket CreateAuthenticationTicket(Account298 user)
        {
            UserInfo userInfo = CreateUserContextFromUser(user);

            var ticket = new FormsAuthenticationTicket(
                1,
                user.Username,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                false,
                userInfo.ToString());

            return ticket;
        }

        private static UserInfo CreateUserContextFromUser(Account298 user)
        {
            var userContext = new UserInfo
            {
                UserId = user.id_,
                DisplayName = user.Fullname,
                UserIdentifier = user.Email,
                RoleName = user.Roles,
                IsAdmin = user.IsAdmin,
            };

            return userContext;
        }

        public string CurrentUserName
        {
            get
            {
                string userName = string.Empty;

                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    userName = HttpContext.Current.User.Identity.Name.Split('|')[0];
                }

                return userName;
            }
        }
    }
}