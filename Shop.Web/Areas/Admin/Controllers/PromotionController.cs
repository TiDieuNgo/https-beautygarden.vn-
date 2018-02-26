using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using LinqToExcel;
using Newtonsoft.Json;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Areas.Admin.Models;
using Shop.Web.Controllers;
using Shop.Web.Core.ActionFilters;
using Shop.Web.Core.Helper;
using Shop.Web.Core.Models;

namespace Shop.Admin.Controllers
{
    [ShopAuthorize]
    public class PromotionController : BaseController
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMenuRepository _menuRepository;
        private readonly IPromotionDetailRepository _promotionDetailRepository;
        private readonly IMenuOptionRepository _menuOptionRepository;
        private const int pageSize = 20;
        private readonly HttpContextBase _httpContext;

        public PromotionController(
            IUnitOfWork unitOfWork, IPromotionRepository promotionRepository, IMenuRepository menuRepository, IPromotionDetailRepository promotionDetailRepository, IMenuOptionRepository menuOptionRepository, HttpContextBase httpContext)
        {
            _unitOfWork = unitOfWork;
            _promotionRepository = promotionRepository;
            _menuRepository = menuRepository;
            _promotionDetailRepository = promotionDetailRepository;
            _menuOptionRepository = menuOptionRepository;
            _httpContext = httpContext;
        }

        public ActionResult Index(int? page)
        {
            if (!CheckRole(_httpContext, int.Parse(Roles.Khuyen_mai_View)))
                return View("_NoAuthor");

            return View("Index");
        }

        public JsonResult GetAll()
        {
            IList<PromotionMapping.Promotion> promotions = _promotionRepository.GetAllPromotions();
            return Json(promotions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPromotionDetails(int promotionId)
        {
            IList<PromotionDetailMapping.PromotionDetail> details =
                _promotionDetailRepository.GetByPromotionId(promotionId);
            return Json(details, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult Save(PromotionModel.PromotionSaveModel model)
        {
            Promotion promotion;
            IList<PromotionDetail> promotionDetails = JsonConvert.DeserializeObject<IList<PromotionDetail>>(model.Details);

            DateTime start = DateTimeHelper.ConvertToLongDay(model.StartDay);
            DateTime end = DateTimeHelper.ConvertToLongDay(model.EndDay);

            if (model.id_ == 0)
            {
                promotion = new Promotion()
                {
                    Active = model.Active,
                    Discount = model.Discount,
                    EndDay = end,
                    StartDay = start,
                    IdUserOk = 0,
                    ProductCategoryIds = model.ProductCategoryIds,
                    ProductIds = model.ProductIds,
                    Region = model.Region,
                    Title = model.Title,
                    IdUser = 15
                };

                _promotionRepository.Add(promotion);
                _unitOfWork.Commit();

            }
            else
            {
                promotion = _promotionRepository.GetById(model.id_);
                promotion.Active = model.Active;
                promotion.Discount = model.Discount;
                promotion.EndDay = end;
                promotion.StartDay = start;
                promotion.ProductCategoryIds = model.ProductCategoryIds;
                promotion.ProductIds = model.ProductIds;
                promotion.Region = model.Region;
                promotion.Title = model.Title;

                _promotionRepository.Update(promotion);
            }

            #region

            IList<PromotionDetail> oldDetails = model.id_ != 0
                                                    ? _promotionDetailRepository.GetMany(o => o.PromotionId == model.id_).ToList()
                                                    : new List<PromotionDetail>();

            foreach (var dt in promotionDetails)
            {
                if(dt.id_==0)
                {
                    PromotionDetail promotionDetail=new PromotionDetail()
                                                        {
                                                            Percent = dt.Percent,
                                                            ProductId = dt.ProductId,
                                                            PriceDiscount = dt.PriceDiscount,
                                                            PromotionId = promotion.id_,
                                                            Price = dt.Price
                                                           
                                                        };
                    _promotionDetailRepository.Add(promotionDetail);
                }else
                {
                    PromotionDetail promotionDetail = oldDetails.FirstOrDefault(o => o.id_ == dt.id_);
                    if(promotionDetail!=null)
                    {
                        promotionDetail.Percent = dt.Percent;
                        promotionDetail.PriceDiscount = dt.PriceDiscount;
                        promotionDetail.Price = dt.Price;
                        _promotionDetailRepository.Update(promotionDetail);
                    }
                }
            }
            #endregion

            _promotionRepository.ClearBannerCache();
            _unitOfWork.Commit();

            return Json(new
            {
                PromotionId = promotion.id_
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(PromotionModel.PromotionSaveModel model)
        {
            _promotionRepository.Delete(o => o.id_ == model.id_);
            _promotionRepository.ClearBannerCache();
            _unitOfWork.Commit();
            return Json(new {}, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult UploadImportFile()
        {

            string sPath = Server.MapPath("~/Upload/");
            string filePath = "";

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    string guid = Guid.NewGuid().ToString();
                    string fileName = string.Format("{0}{1}", guid, Path.GetExtension(hpf.FileName));
                     filePath = sPath + fileName;
                    hpf.SaveAs(filePath);
                   
                }
            }

           // string sheetName = "Sheet";
            var excel = new ExcelQueryFactory(filePath);
            var worksheetNames = excel.GetWorksheetNames();
            string name = "";
            if (worksheetNames.Any())
                name = worksheetNames.FirstOrDefault();

            excel.AddMapping<PromotionModel.ImportDataModel>(o => o.Code, "Mã hàng");
            excel.AddMapping<PromotionModel.ImportDataModel>(o => o.Price, "Giá Thiết Lập");

            var datas = (from o in excel.Worksheet<PromotionModel.ImportDataModel>(name)
                         select o).ToList();

            IList<PromotionDetailMapping.PromotionDetail> promotionDetails =new List<PromotionDetailMapping.PromotionDetail>();
            if(datas.Any())
            {
                var groups = datas.Where(o=>!string.IsNullOrEmpty(o.Code)).GroupBy(o => o.Code).Select(o => new
                                                                        {
                                                                            Code = o.Key,
                                                                            Price = o.FirstOrDefault().Price
                                                                        });
                IList<string> codes = groups.Select(o => o.Code.Trim()).ToList();
                string a = string.Join(",", codes);
               promotionDetails =
                    _menuOptionRepository.GetForPromotionDetailByCodes(codes);

                if(promotionDetails.Any())
                {
                    foreach (var promotionDetail in promotionDetails)
                    {
                        var o = groups.FirstOrDefault(g => g.Code.Equals(promotionDetail.Code));
                        if(o!=null && o.Price != null)
                        {
                            promotionDetail.PriceDiscount = Convert.ToDecimal(o.Price.Replace(",", ""));
                            promotionDetail.Percent = 100 - Math.Floor(promotionDetail.PriceDiscount * 100 / promotionDetail.Price);
                        }
                    }
                }

            }
            return Json(new
            {
                products = JsonConvert.SerializeObject(promotionDetails)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveDetail(PromotionDetail model)
        {
            _promotionDetailRepository.Delete(o=>o.id_==model.id_);
            _promotionRepository.ClearBannerCache();
            _unitOfWork.Commit();
            return Json(new {}, JsonRequestBehavior.AllowGet);
        }

        #region export
        public ActionResult Export(int promotionId)
        {
            PromotionModel.ExportModel model = new PromotionModel.ExportModel()
            {
                PromotionId = promotionId,
                Report = new XtraReport()
            };
            return View("Print",model);
        }

        public PartialViewResult ReportViewerPartial(int promotionId)
        {
            PromotionModel.ExportModel model = new PromotionModel.ExportModel()
            {
                PromotionId = promotionId,
                Report = GetReport(promotionId)
            };
            return PartialView(model);
        }

        public ActionResult ExportReportViewer(int promotionId)
        {
            return ReportViewerExtension.ExportTo(GetReport(promotionId));
        }

        private XtraReport GetReport(int promotionId)
        {
            Promotion promotion = _promotionRepository.GetById(promotionId);
            IList<MenuCategoryForAddPromotionmapping> products = new List<MenuCategoryForAddPromotionmapping>();

            if (!string.IsNullOrEmpty(promotion.ProductIds))
            {
                IList<int> productIds = promotion.ProductIds.Split(',').Select(o => Convert.ToInt32(o)).ToList();
                if (productIds.Any())
                    products = _menuRepository.GetByIds(productIds);
            }


            var report = new Shop.Web.Areas.Admin.ReportDesign.PromotionProduct.Report();
            var infoTable = report.dataSet1.Info;
            var infoRow = infoTable.NewInfoRow();
            infoRow.Range = string.Format("Từ ngày {0} đến {1}", promotion.StartDay.ToString("dd/MM/yyyy HH:mm"),
                promotion.EndDay.ToString("dd/MM/yyyy HH:mm"));
            infoTable.Rows.Add(infoRow);

            if (products.Any())
            {
                var detailTable = report.dataSet1.Detail;
                int stt = 0;
                foreach (var product in products)
                {
                    stt++;
                    var detailRow = detailTable.NewDetailRow();
                    detailRow.Stt = stt + "";
                    detailRow.Code = product.ProductId + "";
                    detailRow.Name = product.Name;
                    detailRow.Price = product.Price.HasValue ? product.Price.Value.ToString("#,###,###,###") : "";

                    detailRow.PriceDiscount = product.Price.HasValue
                        ? (product.Price.Value - (product.Price.Value*promotion.Discount/100)).ToString("#,###,###,###")
                        : "";

                    detailTable.Rows.Add(detailRow);
                }
            }
            return report;

        }
        #endregion

    }
}
