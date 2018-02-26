using System.Web.Mvc;
using Shop.Web.ShoppingAds;
using Shop.Web.Sitemap;

namespace Shop.Web.Model
{
    public class ShoppingAdsNodeRepository : IShoppingAdsNodeRepository
    {
        public string SetShoppingAdsNodes(UrlHelper urlHelper, string path)
        {
            return clsShoppingAds.SetShoppingAds(urlHelper, path);
        }
    }
}