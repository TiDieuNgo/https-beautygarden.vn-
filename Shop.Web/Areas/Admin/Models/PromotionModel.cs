using DevExpress.XtraReports.UI;

namespace Shop.Web.Areas.Admin.Models
{
    public class PromotionModel
    {
        public class PromotionSaveModel
        {
            public int id_ { get; set; }
            public string Title { get; set; }
            public string StartDay { get; set; }
            public string EndDay { get; set; }
            public short Discount { get; set; }
            public string ProductCategoryIds { get; set; }
            public string ProductIds { get; set; }
            public bool Active { get; set; }
            public string Region { get; set; }
            public string Details { get; set; }
        }

        public class ExportModel
        {
            public int PromotionId { get; set; }
            public XtraReport Report { get; set; }
        }

        public class ImportDataModel
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public string Price { get; set; }
            public string PricePromotion { get; set; }
        }
    }
}