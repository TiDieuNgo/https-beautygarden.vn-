using System.Web.Mvc;


namespace Shop.Web.Controllers
{
    public class TagsController : Controller
    {
        //static readonly ISitemapNodeRepository repositorySitemap = new SitemapNodeRepository(); 

        public ActionResult Index()
        {
           // string xml = repositorySitemap.SetSitemapNodes(this.Url, HostingEnvironment.ApplicationPhysicalPath + "sitemap.xml");
            return View();
       }
        
    }
}
