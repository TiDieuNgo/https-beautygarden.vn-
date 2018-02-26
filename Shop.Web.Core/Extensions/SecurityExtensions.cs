using System.Security.Principal;
using Shop.Web.Core.Models;

namespace Shop.Web.Core.Extensions
{
    public static class SecurityExtensions
    {
      public static string Name(this IPrincipal user)
        {
            return user.Identity.Name;
        }

        public static bool InAnyRole(this IPrincipal user, params string[] roles)
        {
            foreach (string role in roles)
            {
                if (user.IsInRole(role)) return true;
            }
            return false;
        }
        public static ShopUser GetShopUser(this IPrincipal principal)
        {
            if (principal.Identity is ShopUser)
                return (ShopUser)principal.Identity;
            else
                return new ShopUser(string.Empty, new UserInfo());
        }
    }    
}
