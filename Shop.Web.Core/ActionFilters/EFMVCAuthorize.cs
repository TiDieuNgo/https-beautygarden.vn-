using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Shop.Web.Core.ActionFilters
{
    public class ShopAuthorize : AuthorizeAttribute
    {
        public ShopAuthorize(params string[] roles)
        {
            this.Roles = String.Join(", ", roles);
        }
    }
}
