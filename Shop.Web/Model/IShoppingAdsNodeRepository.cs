using System.Web.Mvc;

namespace Shop.Web.Model
{
    interface IShoppingAdsNodeRepository
    {
        string SetShoppingAdsNodes(UrlHelper urlHelper, string path);
    }
}