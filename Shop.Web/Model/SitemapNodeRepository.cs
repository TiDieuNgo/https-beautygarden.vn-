using System.Web.Mvc;
using Shop.Web.Sitemap;

namespace Shop.Web.Model
{
    public class SitemapNodeRepository : ISitemapNodeRepository
    {
        public string SetSitemapNodes(UrlHelper urlHelper, string path)
        {
            return clsSitemap.SetSitemap(urlHelper, path);
        }
    } 
}