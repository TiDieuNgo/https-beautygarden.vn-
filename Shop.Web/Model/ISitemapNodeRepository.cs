using System.Web.Mvc;

namespace Shop.Web.Model
{
    interface ISitemapNodeRepository
    {
        string SetSitemapNodes(UrlHelper urlHelper, string path);
    }
}