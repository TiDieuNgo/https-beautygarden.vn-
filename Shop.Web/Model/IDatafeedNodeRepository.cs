using System.Web.Mvc;

namespace Shop.Web.Model
{
    interface IDatafeedNodeRepository
    {
        string SetDatafeedNodes(UrlHelper urlHelper, string path);
    }
}