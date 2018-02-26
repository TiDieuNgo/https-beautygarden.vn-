using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.DataAccess.Native.Sql;
using PagedList;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Common;
using Shop.Web.Core.Models;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class FormOrderSalePageController : BaseController
    {
        private readonly IIframeSalepageRepository _iframeSalepageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FormOrderSalePageController(IIframeSalepageRepository iframeSalepageRepository, IUnitOfWork unitOfWork)
        {
            _iframeSalepageRepository = iframeSalepageRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Addnew(IframeSalepage model)
        {
            string barcode = model.Barcode;
            string src ="https://beautygarden.vn/formdathang?barcode=" + barcode;
            string formIframe = string.Format("<iframe class='iframebarcode' src='{0}' width='100%' style='width: 100%; border: 0;height: 307px;'></iframe>", src);
            // tạo Iframe luu gia va gia khuyen mai
            string srcgia = "https://beautygarden.vn/formdathang/GiaSalePage?barcode=" + barcode;
            string formIframegia = string.Format("<iframe class='iframegia' src='{0}' width='100%' style='width: 100%; border: 0;height: 55px;'></iframe>", srcgia);
            // tạo Iframe luu form đặt hàng
            IframeSalepage iframe = new IframeSalepage()
                                    {
                                        Iframe = formIframe,
                                        Barcode = barcode,
                                        IframeGia = formIframegia,
                                        NgayTao = DateTime.Now
                                    };
            _iframeSalepageRepository.Add(iframe);
            _unitOfWork.Commit();
            return RedirectToAction("Index","FrameSalePage");
        }
    }
}
