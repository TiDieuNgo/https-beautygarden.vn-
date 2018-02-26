using System.Web.Mvc;
using Shop.Web.Datafeed;

namespace Shop.Web.Model
{
    public class DatafeedNodeRepository : IDatafeedNodeRepository
    {
        public string SetDatafeedNodes(UrlHelper urlHelper, string path)
        {
            return clsDatafeed.SetDatafeed(urlHelper, path);
        }
    } 
}