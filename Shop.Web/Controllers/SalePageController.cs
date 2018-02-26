using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Core.ActionFilters;

namespace Shop.Web.Controllers
{
    [WhitespaceFilter]
    public class SalePageController : BaseController
    {
        private readonly ISalePageRepository _salePageRepository;

        public SalePageController(ISalePageRepository salePageRepository)
        {
            _salePageRepository = salePageRepository;
        }

        public ActionResult Index()
        {
            IList<SalePage> salePages = _salePageRepository.GetMany(o=>o.Showhide).OrderByDescending(o => o.NgayTao).ToList();
            return View("Index", salePages);
        }

        public ActionResult ChiTiet(string linksalepage)
        {
            SalePage salePage = _salePageRepository.GetMany(o => o.Link == linksalepage).FirstOrDefault();
            return View("salepage", salePage);
        }

        public ActionResult SalePageLoadTinTuc(int IdSalePage)
        {
            SalePage salePage = _salePageRepository.GetMany(o => o.Id == IdSalePage).FirstOrDefault();
            return View("salepageloadtintuc", salePage);
        }
        //ĐANG CHẠY TRÊN TRANG NÀY
        public ActionResult SalePageMoi(string linksalepage)
        {
            SalePage salePage = _salePageRepository.GetMany(o => o.Link == linksalepage).FirstOrDefault();
            return View("salepagemoi", salePage);
        }
        public static string RemarkettingFacebookViewContent(string barcode)
        {
            Menu data = Shop.Web.Model.YKienKhachHangModel.GetMenuByBarcode(barcode);
            StringBuilder JavaScript = new StringBuilder();
            JavaScript = JavaScript.Append("<script>")
                .Append("fbq('track', 'ViewContent', {")
                .Append("content_name:'" + data.NameProduct + "',")
                .Append("content_type:'product',")
                .Append("content_ids:['BGdetail_" + data.id_ + "'],")
                .Append("value:" + data.PricePro + ",")
                .Append("currency:'VND'")
                .Append("});")
                .Append("</script>");
            return JavaScript.ToString();
        }
    }
}
