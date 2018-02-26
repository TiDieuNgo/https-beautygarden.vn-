using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Security.Principal;

namespace Shop.Web.Core.Models
{ 
    [Serializable]
    public class ShopUser : IIdentity
    {
        public ShopUser() { }
        public ShopUser(string name, string displayName, int userId)
        {
            this.Name = name;
            this.DisplayName = displayName;
            this.UserId = userId;
        }
        public ShopUser(string name, string displayName, int userId, string roleName, bool isAdmin)
        {
            this.Name = name;
            this.DisplayName = displayName;
            this.UserId = userId;
            this.RoleName = roleName;
            this.IsAdmin = isAdmin;
        }
        public ShopUser(string name, UserInfo userInfo)
            : this(name, userInfo.DisplayName, userInfo.UserId, userInfo.RoleName, userInfo.IsAdmin)
        {
            if (userInfo == null) throw new ArgumentNullException("userInfo");
            this.UserId = userInfo.UserId;
        }

        public ShopUser(FormsAuthenticationTicket ticket)
            : this(ticket.Name, UserInfo.FromString(ticket.UserData))
        {
            if (ticket == null) throw new ArgumentNullException("ticket");
        }

        public string Name { get; private set; }

        public string AuthenticationType
        {
            get { return "ShopForms"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string DisplayName { get; private set; }
        public string RoleName { get; private set; }
        public int UserId { get; private set; }
        public bool IsAdmin { get; set; }
    }
}
